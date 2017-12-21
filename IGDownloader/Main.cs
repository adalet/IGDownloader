using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IGDownloader
{
    public partial class Main : Form
    {
        SettingsWindow settingsWindow;
        CSRF csrf;

        private bool loggedIn = false;
        private string username;
        private string password;
        private string sessionID;
        private string LoggedOnID;
        private string downloadList;
        private string savedData;

        public string FOLLOWING;
        public string USER_FEED;
        

        public string ACCOUNT_JSON_INFO = "https://www.instagram.com/{username}/?__a=1";
        public string FOLLOWING_INFO = "https://www.instagram.com/graphql/query/?query_id=17874545323001329&id={id}&first={first}";
        public string MEDIA_JSON_INFO = "https://www.instagram.com/p/{code}/?__a=1";
        public string ACCOUNT_MEDIA_INFO = "https://www.instagram.com/graphql/query/?query_id=17888483320059182&variables={%22id%22:%22{username}%22,%22first%22:{first},%22after%22:%22{after}%22}";

        private enum RequestType
        { ACCOUNT_JSON_INFO, FOLLOWING_INFO, MEDIA_JSON_INFO, ACCOUNT_MEDIA_INFO };

        private Dictionary<string, string> followingList = new Dictionary<string, string>();

        public Main()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

            if ((string)Properties.Settings.Default["SaveLocation"] == string.Empty)
            {
                Properties.Settings.Default["SaveLocation"] = Directory.GetCurrentDirectory() + "\\instagram";
            }

            InitializeComponent();
            showStartupInformation();
            settingsWindow = new SettingsWindow();
        }

        private void showStartupInformation()
        {
            writeToConsole("Instagram Image Downloader - Created by Adalet (Daze) (https://github.com/adalet)");
            writeToConsole("Current save directory: " + Properties.Settings.Default["SaveLocation"]);        
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            username = usernameBox.Text;
            password = passwordBox.Text;

            passwordBox.Text = "password"; //Reset so the password length isn't identified

            writeToConsole("Attempting to login...");
            login(username, password);
            
        }

        // Returns the id of the profile given
        private string getProfileID(string name)
        {
            try
            {
                string source = new System.Net.WebClient().DownloadString($"https://www.instagram.com/{name}");
                string pattern = "\"id\": \"(.*)\"";

                string ID = string.Empty;

                foreach (Match item in (new Regex(pattern).Matches(source)))
                {
                    ID = item.Groups[1].Value.Split('"')[0];
                }
                return ID;
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }

        public void login(string user, string password)
        {
            try
            {
                bool loggedOn = false;
                LoggedOnID = getProfileID(user);
                csrf = new CSRF(); // CSRF token is only necessary when the user is logging in or requesting a private profile
                csrf.generateCSRF(); // Only needs to be generated once per session?
                string CSRF = csrf.CSRF_Token;
                string post = "username=" + user + "&password=" + password;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.instagram.com/accounts/login/ajax/");
                request.Method = "POST";
                request.Host = "www.instagram.com";
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Accept = "*/*";
                request.Referer = "https://www.instagram.com/accounts/login/";
                request.Headers.Add("Origin", "https://www.instagram.com");
                request.Headers.Add("X-Instagram-AJAX", "1");
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.Headers.Add("X-CSRFToken", CSRF);
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("Cookie", $"mid=VlW1MgAEAAEgkDVr8Pa-nokWXqCF; csrftoken={CSRF}; ig_pr=1; ig_vw=1160");

                byte[] postBytes = Encoding.ASCII.GetBytes(post);
                request.ContentLength = postBytes.Length;
                Stream requestStream = request.GetRequestStream();

                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string html = new StreamReader(response.GetResponseStream()).ReadToEnd();
                loggedOn = html.Contains("\"authenticated\": true");
                var cookieTitle = "sessionid";
                var cookie = response.Headers.GetValues("Set-Cookie").First(x => x.StartsWith(cookieTitle));
                sessionID = cookie;
                string[] splitter = sessionID.Split(new string[] { "sessionid=" }, StringSplitOptions.None);
                sessionID = splitter[1];
                writeToConsole("Successfully logged in!");
                loggedInStatus.Text = "Status: Logged in with " + username;
                loggedIn = true;
            }
            catch (Exception ex)
            {
                writeToConsole("Error, could not log in successfully. Please check your username/password.");
                loggedIn = false; // Possible bug since variables arent reset?
            }
        }

        private string downloadMediaList(string info, RequestType requestType, string positionIndex)
        {
            string id;
            string CSRF = csrf.CSRF_Token;
            string post = $"";

            HttpWebRequest request;

            if (requestType == RequestType.ACCOUNT_MEDIA_INFO)
            {
                // 17888483320059182 = getMediaList
                string url = ACCOUNT_MEDIA_INFO;
                id = getProfileID(info);
                url = url.Replace("{username}", id);
                url = url.Replace("{first}", "1000");
                url = url.Replace("{after}", positionIndex);
                request = (HttpWebRequest)WebRequest.Create(url);
            }
            else if (requestType == RequestType.ACCOUNT_JSON_INFO)
            {
                string url = ACCOUNT_JSON_INFO;
                url = url.Replace("{username}", info);
                request = (HttpWebRequest)WebRequest.Create(url);
            }
            else if (requestType == RequestType.MEDIA_JSON_INFO)
            {
                string url = MEDIA_JSON_INFO;
                url = url.Replace("{code}", info);
                request = (HttpWebRequest)WebRequest.Create(url);
            }
            else
            {
                id = getProfileID(info);
                request = (HttpWebRequest)WebRequest.Create("https://www.instagram.com/graphql/query/?query_id=17888483320059182&variables={%22id%22:%22" + id + "%22,%22first%22:1000,%22after%22:%22" + positionIndex + "%22}" );
            }

            request.Method = "GET";
            request.Host = "www.instagram.com";
            request.KeepAlive = true;
            request.ContentLength = 0;
            request.Accept = "*/*";
            request.Headers.Add("Origin", "https://www.instagram.com");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("X-Instagram-AJAX", "1");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            request.Headers.Add("X-CSRFToken", CSRF);
            request.Referer = "https://instagram.com/" + username + "/";
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            request.Headers.Add("Cookie", $"sessionid={sessionID}; s_network=; ig_pr=0.8999999761581421; ig_vw=1517; csrftoken={csrf}; ds_user_id={LoggedOnID}");
            request.AutomaticDecompression = DecompressionMethods.GZip;
            byte[] postBytes = Encoding.ASCII.GetBytes(post);
            request.ContentLength = postBytes.Length;

            string json = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                using (StreamReader read = new StreamReader(stream))
                {
                    json = read.ReadToEnd();
                }
            }
            catch(WebException exception)
            {
                writeToConsole(exception.Message);
            }
            
            return json;
        }

        private void downloadFollowingList(string username)
        {
            try
            {
                string id = getProfileID(username);
                string CSRF = csrf.CSRF_Token;
                string post = $"";
                string url = FOLLOWING_INFO;
                url = url.Replace("{id}", id);
                url = url.Replace("{first}", "5000"); //5k might not work, 1k is probably the max for a current page

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.instagram.com/graphql/query/?query_id=17874545323001329&variables={\"id\":\"" + id + "%22,%22first%22:5000}");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Host = "www.instagram.com";
                request.KeepAlive = true;
                request.ContentLength = 0;
                request.Accept = "*/*";
                request.Headers.Add("Origin", "https://www.instagram.com");
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.Headers.Add("X-Instagram-AJAX", "1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
                request.Headers.Add("X-CSRFToken", CSRF);
                request.Referer = "https://instagram.com/" + username + "/";
                request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                request.Headers.Add("Cookie", $"sessionid={sessionID}; s_network=; ig_pr=0.8999999761581421; ig_vw=1517; csrftoken={csrf}; ds_user_id={LoggedOnID}");
                request.AutomaticDecompression = DecompressionMethods.GZip;
                byte[] postBytes = Encoding.ASCII.GetBytes(post);
                request.ContentLength = postBytes.Length;

                string json = "";
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    using (StreamReader read = new StreamReader(stream))
                    {
                        json = read.ReadToEnd();
                    }
                }
                catch (WebException exception)
                {
                    writeToConsole(exception.Message);
                }

                string tempID = "";
                string tempUsername = "";
                JsonTextReader reader = new JsonTextReader(new StringReader(json));

                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                        {
                            reader.Read();
                            tempID = (string)reader.Value;
                        }
                        if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "username")
                        {
                            reader.Read();
                            listBox.Items.Add((string)reader.Value);
                            tempUsername = (string)reader.Value;
                            followingList.Add(tempUsername, tempID);
                        }
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Throw: {ex}");
            }
        }

        // Loads a local textfile into the checkbox list.
        // Checks if the username is valid and notifies the user if there is no associated ID
        private void loadTxtFile(string textFile)
        {
            List<string> lines = new List<string>();
            using (StreamReader r = new StreamReader(textFile))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            WebClient client = new WebClient();
            string myJSON;
            for (int x = 0; x < lines.Count; x++)
            {
                try
                {
                    myJSON = client.DownloadString("https://www.instagram.com/" + lines[x] + "/?__a=1");
                }
                catch (WebException e)
                {
                    writeToConsole("Error for user " + lines[x] + ":" + e.Message);
                    continue;
                }

                JsonTextReader reader = new JsonTextReader(new StringReader(myJSON));
                string tempID = "";

                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                        {
                            reader.Read();
                            if (reader.TokenType == JsonToken.String)
                            {
                                string url2 = (string)reader.Value;
                                string[] tokens = url2.Split(new[] { "/" }, StringSplitOptions.None);
                                string filename = tokens.Last<string>();

                                tempID = (string)reader.Value;
                                if (!followingList.ContainsKey(lines[x]))
                                {
                                    listBox.Items.Add(lines[x]);
                                    followingList.Add(lines[x], tempID);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void fetchButton_Click(object sender, EventArgs e)
        {
            if (listBox.Items.Count > 0)
            {
                listBox.Items.Clear();
            }
            if (followingList.Count > 0)
            {
                followingList.Clear();
            }

            writeToConsole("Populating the control list...");

            if (fetchFollowingCheckBox.Checked)
            {
                if (loggedIn == true)
                {
                    downloadFollowingList(username);
                }
                else
                {
                    writeToConsole("Error, you are not logged in.");
                }
            }

            if (fetchLocalCheckBox.Checked)
            {
                if (!File.Exists("following.txt"))
                {
                    writeToConsole("Warning: Could not find following.txt, recreating...");
                    writeToConsole("Warning: Manually edit the following.txt file with your own accounts and refetch the list.");
                    
                    StreamWriter writer = new StreamWriter("following.txt");
                    writer.Write("instagram");
                    writer.Close();
                }
                loadTxtFile("following.txt");
            }

            if (!fetchLocalCheckBox.Checked && !fetchFollowingCheckBox.Checked)
            {
                writeToConsole("Error: Please place a check mark on which list you want to fetch");
               
            }

            writeToConsole("Finished populating the control list...");

            if (listBox.Items.Count == 0)
            {
                selectAllCheck.Checked = false;
                selectAllCheck.Visible = false;
            }
            else
            {
                selectAllCheck.Visible = true;
            }
        }

        private void writeToConsole(string message)
        {
            if (consoleBox.InvokeRequired)
            {
                consoleBox.Invoke(new MethodInvoker(delegate
                {
                    consoleBox.Text += "\n" + message;
                    consoleBox.SelectionStart = consoleBox.Text.Length;
                    consoleBox.ScrollToCaret();
                }));
            }
            else
            {
                consoleBox.Text += "\n" + message;
                consoleBox.SelectionStart = consoleBox.Text.Length;
                consoleBox.ScrollToCaret();
            }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (listBox.CheckedItems.Count <= 0)
            {
                writeToConsole("Error: You didn't select anyone from the list.");
                return;
            }

            lockAllControls();
            downloadThread.RunWorkerAsync();
        }

        // TODO: Rewrite the download functions, many instances of unnecessary checks are performed
        private void downloadSidecar(ImageAttributes attributes, string myJSON, string username)
        {  
            WebClient client = new WebClient();
            string id = "";
            string imageType = "";
            JsonTextReader reader = new JsonTextReader(new StringReader(myJSON));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "shortcode_media")
                    {
                        while (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "edge_sidecar_to_children")
                        {
                            reader.Read();
                        }

                        while (reader.Read())
                        {
                            if(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "__typename")
                            {
                                reader.Read();
                                imageType = (string)reader.Value;
                            }

                            if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                            {
                                reader.Read();
                                id = (string)reader.Value;
                            }

                            if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "display_url")
                            {
                                reader.Read();

                                string download_link = (string)reader.Value;
                                string filename = (string)Properties.Settings.Default["FilenameTemplate"];
                                filename = filename.Replace(FilenameTemplate.DATE, attributes.DATETIME.ToString("yyyy-MM-dd"));
                                filename = filename.Replace(FilenameTemplate.ID, id);

                                // Files are not private, once you obtain the link, it can be downloaded
                                client.DownloadFile(new Uri(download_link), Properties.Settings.Default["SaveLocation"] + "/" + username + "/" + filename + ".jpg");

                            }

                            if (imageType == "GraphVideo" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "video_url")
                            {
                                reader.Read();

                                string download_link = (string)reader.Value;
                                string filename = (string)Properties.Settings.Default["FilenameTemplate"];
                                filename = filename.Replace(FilenameTemplate.DATE, attributes.DATETIME.ToString("yyyy-MM-dd"));
                                filename = filename.Replace(FilenameTemplate.ID, id);

                                string videoJsonURL = "https://www.instagram.com/p/" + attributes.SHORTCODE + "?__a=1";
                                string videoJson = "";
                                try
                                {
                                    videoJson = client.DownloadString(videoJsonURL);
                                }
                                catch (WebException e)
                                {
                                    writeToConsole(e.Message);
                                }

                                string videoURL = getVideoDownload(videoJson);

                                try
                                {
                                    client.DownloadFile(new Uri(attributes.DISPLAY_URL), Properties.Settings.Default["SaveLocation"] + "/" + username + "/" + filename + ".jpg");
                                    if (videoURL != string.Empty)
                                        client.DownloadFile(new Uri(videoURL), Properties.Settings.Default["SaveLocation"] + "/" + username + "/" + filename + ".mp4");
                                }
                                catch (WebException e)
                                {
                                    writeToConsole(e.Message);
                                }

                            }
                        }
                    }
                }
            }
        }

        public string getVideoDownload(string myJSON)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(myJSON));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "shortcode_media")
                    {
                        while (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "id")
                        {
                            reader.Read();
                        }

                        reader.Read();

                        if (reader.TokenType == JsonToken.String)
                        {
                            string id = (string)reader.Value;
                        }

                        while (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "video_url")
                        {
                            reader.Read();
                        }

                        reader.Read();

                        if (reader.TokenType == JsonToken.String)
                        {
                            string download_link = (string)reader.Value;
                            return download_link;
                        }
                    }
                }
            }
            return "";
        }

        // Checks if the account is private and if the user is able to see it
        private bool canSeeAccount(string username)
        {
            WebClient client = new WebClient();

            string url = "https://www.instagram.com/" + username + "?__a=1";
            string myJSON = "";
            try
            {
                myJSON = client.DownloadString(url);
            }
            catch (WebException e)
            {
                //MessageBox.Show(e.Message);
                writeToConsole(e.Message);
            }

            bool followedByViewer = false;
            bool isPrivate = false;
            JsonTextReader reader = new JsonTextReader(new StringReader(myJSON));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "followed_by_viewer")
                    {
                        reader.Read();

                        if (reader.TokenType == JsonToken.Boolean)
                        {
                            followedByViewer = (bool)reader.Value;
                        }
                    }
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "is_private")
                    {
                        reader.Read();

                        if (reader.TokenType == JsonToken.Boolean)
                        {
                            isPrivate = (bool)reader.Value;
                        }
                    }
                }
            }

            if (isPrivate && followedByViewer)
            {
                return true;
            }
            else if (isPrivate && !followedByViewer)
            {
                return false;
            }
            return true;
        }

        // TODO: Rewrite
        // Downloads a json file based on if the user is logged in or not. Reads the json file and downloads the posts accordingly.
        public void downloadAlbums(string url, string username, string positionIndex)
        {
            ImageAttributes imageAttributes = new ImageAttributes();
            WebClient client = new WebClient();
            string userID = getProfileID(username);

            string myJSON = "";

            if (loggedIn)
            {
                myJSON = downloadMediaList(username, RequestType.ACCOUNT_MEDIA_INFO, positionIndex);
            }
            else
            {
                if (!canSeeAccount(username))
                {
                    writeToConsole(username + " is private. Make sure you are logged in and following this user to view it.");
                    return;
                }
                try
                {
                    myJSON = client.DownloadString(url);
                }
                catch (WebException e)
                {
                    //MessageBox.Show(e.Message);
                    writeToConsole(e.Message);
                    return;
                }
            }

            
            JsonTextReader reader = new JsonTextReader(new StringReader(myJSON));

            System.IO.Directory.CreateDirectory(Properties.Settings.Default["SaveLocation"] + "/" + username);
            FileStream saveFile = new FileStream(Properties.Settings.Default["SaveLocation"] + "/" + username + "/saved.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader saveRead = new StreamReader(saveFile);
            
            savedData = saveRead.ReadToEnd();
            saveRead.Close();
            saveFile.Close();
            downloadList = "";
            long totalImages = 0;
            long currentBatchSize = 0;
            int count = 0;
            bool foundImageCount = false;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "count" && !foundImageCount)
                    {
                        reader.Read();
                        totalImages = (long)reader.Value;
                        foundImageCount = true; // Image count is the first property but "count" appears multiple times
                    }
                    if(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "has_next_page")
                    {
                        reader.Read();
                        if((bool)reader.Value == true)
                        {
                            reader.Read();
                            reader.Read();
                            currentBatchSize = 1000;
                            string nextPageIndex = (string)reader.Value;
                            string nextPageURL = "https://www.instagram.com/graphql/query/?query_id=17888483320059182&variables={%22id%22:%22" + userID + "%22,%22first%22:1000,%22after%22:%22" + nextPageIndex + "%22}";
                            downloadAlbums(nextPageURL, username, nextPageIndex);
                        }
                        else
                        {
                            currentBatchSize = totalImages % 1000;
                            double batches = Math.Floor((double)totalImages / 1000) + 1;
                            if (totalImages > 1000)
                            {
                                writeToConsole("Total posts found: " + totalImages + ". Splitting into " + batches + " batches.");
                            }
                            else
                            {
                                writeToConsole("Total posts found: " + totalImages + ".");
                            }
                            
                            writeToConsole("Downloaded: 0/" + currentBatchSize);
                        }
                    }

                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "node")
                    {
                        while (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "id")
                        {
                            reader.Read();
                        }

                        reader.Read();

                        if (reader.TokenType == JsonToken.String)
                        {
                            imageAttributes.ID = (string)reader.Value;
                        }

                        while (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "__typename")
                        {
                            reader.Read();
                        }

                        reader.Read();

                        if (reader.TokenType == JsonToken.String)
                        {
                            imageAttributes.IMAGE_TYPE = (string)reader.Value;
                        }

                        while (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "shortcode")
                        {
                            reader.Read();
                        }

                        reader.Read();

                        if (reader.TokenType == JsonToken.String)
                        {
                            imageAttributes.SHORTCODE = (string)reader.Value;
                        }

                        while (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "taken_at_timestamp")
                        {
                            reader.Read();
                        }

                        reader.Read();

                        if (reader.TokenType == JsonToken.Integer)
                        {
                            imageAttributes.TIMESTAMP = (long)reader.Value;
                            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                            DateTime date = start.AddSeconds(imageAttributes.TIMESTAMP).ToLocalTime();
                            imageAttributes.DATETIME = date;
                        }

                        while (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != "display_url")
                        {
                            reader.Read();
                        }

                        reader.Read();

                        if (reader.TokenType == JsonToken.String)
                        {
                            imageAttributes.DISPLAY_URL = (string)reader.Value;
                        }

                        string filename = (string)Properties.Settings.Default["FilenameTemplate"];

                        filename = filename.Replace(FilenameTemplate.DATE, imageAttributes.DATETIME.ToString("yyyy-MM-dd"));
                        filename = filename.Replace(FilenameTemplate.ID, imageAttributes.ID);

                        if (!savedData.Contains(imageAttributes.ID))
                        {
                            try
                            {
                                if (imageAttributes.IMAGE_TYPE == "GraphImage")
                                {
                                    client.DownloadFile(new Uri(imageAttributes.DISPLAY_URL), Properties.Settings.Default["SaveLocation"] + "/" + username + "/" + filename + ".jpg");
                                }
                                else if (imageAttributes.IMAGE_TYPE == "GraphSidecar")
                                {
                                    if (!loggedIn)
                                    {
                                        string sidecarJsonURL = "https://www.instagram.com/p/" + imageAttributes.SHORTCODE + "?__a=1";
                                        string sidecarJSON;
                                        try
                                        {
                                            sidecarJSON = client.DownloadString(sidecarJsonURL);
                                        }
                                        catch (WebException e)
                                        {
                                            writeToConsole(e.Message);
                                            return;
                                        }
                                        downloadSidecar(imageAttributes, sidecarJSON, username);
                                    }
                                    else
                                    {
                                        string jsonInfo = downloadMediaList(imageAttributes.SHORTCODE, RequestType.MEDIA_JSON_INFO, string.Empty);
                                        downloadSidecar(imageAttributes, jsonInfo, username);
                                    }

                                    // DEAL WITH SIDECAR IMAGES
                                }
                                else if (imageAttributes.IMAGE_TYPE == "GraphVideo")
                                {
                                    if (!loggedIn)
                                    {
                                        string videoJsonURL = "https://www.instagram.com/p/" + imageAttributes.SHORTCODE + "?__a=1";
                                        string videoJson = "";
                                        try
                                        {
                                            videoJson = client.DownloadString(videoJsonURL);
                                        }
                                        catch (WebException e)
                                        {
                                            writeToConsole(e.Message);
                                        }

                                        string videoURL = getVideoDownload(videoJson);

                                        client.DownloadFile(new Uri(imageAttributes.DISPLAY_URL), Properties.Settings.Default["SaveLocation"] + "/" + username + "/" + filename + ".jpg");
                                        if (videoURL != string.Empty)
                                            client.DownloadFile(new Uri(videoURL), Properties.Settings.Default["SaveLocation"] + "/" + username + "/" + filename + ".mp4");
                                    }
                                    else
                                    {
                                        string jsonInfo = downloadMediaList(imageAttributes.SHORTCODE, RequestType.MEDIA_JSON_INFO, string.Empty);
                                        string videoURL = getVideoDownload(jsonInfo);
                                        imageAttributes.VIDEO_URL = videoURL;

                                        if (!savedData.Contains(imageAttributes.ID))
                                        {
                                            client.DownloadFile(new Uri(imageAttributes.DISPLAY_URL), Properties.Settings.Default["SaveLocation"] + "/" + username + "/" + filename + ".jpg");
                                            if (videoURL != string.Empty)
                                                client.DownloadFile(new Uri(videoURL), Properties.Settings.Default["SaveLocation"] + "/" + username + "/" + filename + ".mp4");
                                        }
                                    }
                                }
                                downloadList += imageAttributes.ID + "\n";

                            }
                            catch (WebException e)
                            {
                                if(imageAttributes.IMAGE_TYPE == "GraphVideo")
                                    writeToConsole("Failed to download video: " + imageAttributes.VIDEO_URL);
                                else
                                    writeToConsole("Failed to download image: " + imageAttributes.DISPLAY_URL);
                                writeToConsole(e.Message);
                                writeToConsole("");
                            }                           
                        }
                        count++;
                        updateFileCount("Downloaded: " + count + "/" + currentBatchSize);                      
                    }
                }
            }

            if (downloadList != "")
            {
                saveFile = new FileStream(Properties.Settings.Default["SaveLocation"] + "/" + username + "/saved.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                StreamWriter saveWrite = new StreamWriter(saveFile);
                saveWrite.Write(downloadList);
                saveWrite.Flush();
                saveWrite.Close();
                saveFile.Close();
            }

            
            writeToConsole("Finished downloading files for " + username);
        }

        // Updates the textbox with the current image count as it downloads
        // Can't figure out a better way to replace text in the textbox
        // (Maybe store all text in a List<string> before outputting to textbox?)
        public void updateFileCount(string text)
        {
            if (consoleBox.InvokeRequired)
            {
                consoleBox.Invoke(new MethodInvoker(delegate
                {
                    int location = consoleBox.Lines.Length;
                    string temp = consoleBox.Lines[location - 1];

                    consoleBox.SelectionStart = consoleBox.Text.Length - temp.Length;
                    consoleBox.SelectionLength = temp.Length;
                    consoleBox.SelectedText = text;
                }));
            }
            else
            {
                int location = consoleBox.Lines.Length;
                string temp = consoleBox.Lines[location - 1];

                consoleBox.SelectionStart = consoleBox.Text.Length - temp.Length;
                consoleBox.SelectionLength = temp.Length;
                consoleBox.SelectedText = text;
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            //this.Enabled = false;
            
            settingsWindow.StartPosition = FormStartPosition.CenterParent;
            settingsWindow.ShowDialog(this);
        }

        private void downloadThread_DoWork(object sender, DoWorkEventArgs e)
        {          
            List<string> checkedItems = listBox.CheckedItems.Cast<string>().ToList();
            string url = "";

            for (int x = 0; x < checkedItems.Count; x++)
            {
                try
                {
                    if (followingList[checkedItems[x]] == null)
                    {
                    }
                }
                catch (KeyNotFoundException notFound)
                {
                    writeToConsole(notFound.Message);
                    continue;
                }

                string tempID = followingList[checkedItems[x]];

                url = ACCOUNT_MEDIA_INFO;
  
                url = url.Replace("{username}", tempID);
                url = url.Replace("{first}", "1000");
                url = url.Replace("{after}", "1");

                //url = "https://www.instagram.com/graphql/query/?query_id=17888483320059182&variables={%22id%22:%22" + tempID + "%22,%22first%22:50000}";
                writeToConsole("Downloading files for " + checkedItems[x]);
                downloadAlbums(url, checkedItems[x], "1");
            }
        }

        private void selectAllCheck_CheckStateChanged(object sender, EventArgs e)
        {
            if (selectAllCheck.Checked == false)
            {
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    listBox.SetItemChecked(i, false);
                }
            }

            if (selectAllCheck.Checked == true)
            {
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    listBox.SetItemChecked(i, true);
                }
            }
        }

        private void lockAllControls()
        {
            downloadButton.Enabled = false;
            fetchButton.Enabled = false;
            settingsButton.Enabled = false;
            loginButton.Enabled = false;
            listBox.Enabled = false;
            selectAllCheck.Enabled = false;
            consoleBox.Selectable = false;
        }

        private void unlockAllControls()
        {
            downloadButton.Enabled = true;
            fetchButton.Enabled = true;
            settingsButton.Enabled = true;
            loginButton.Enabled = true;
            listBox.Enabled = true;
            selectAllCheck.Enabled = true;
            consoleBox.Selectable = true;
        }

        private void downloadThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            unlockAllControls();
        }
    }
}