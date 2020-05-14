using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace HttpServer
{
    public class MobileHttpServer
    {
        public string Port { get; set; } = "46467";
        public string IP { get; set; }
        public bool Listening { get; protected set; } = false;


        private HttpListener httpListener;
        private Thread thread;

        public MobileHttpServer()
        {

            IP = GetLocalIP();
            httpListener = new HttpListener();

            if (IP != "" && Port != "")
            {
                httpListener.Prefixes.Add($"http://{IP}:{Port}/");
                httpListener.AuthenticationSchemes = AuthenticationSchemes.None;
                httpListener.IgnoreWriteExceptions = true;
            }
        }

        public void BeginListening()
        {
            if (!Listening)
            {
                thread = new Thread(new ThreadStart(Listen));
                thread.Start();
            }
        }
        public void StopListening()
        {
            if (Listening)
            {
                httpListener.Stop();
                thread.Abort();
                Listening = false;
            }
        }

        protected void Listen()
        {
            try
            {
                // Begin receiving requests
                httpListener.Start();
                Listening = true;

                Debug.WriteLine($"http://{IP}:{Port}/");
                // Loop to allow multiple clients.
                while (Listening)
                {
                    // Wait for someone to connect.
                    HttpListenerContext context = httpListener.GetContext();
                    HttpListenerResponse response = context.Response;

                    // Construct our HTML response into bytes
                    string htmlResponse = ConstructHTML();
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(htmlResponse);

                    // Create an output stream to send our HTML through
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;

                    // Write the output stream over the network
                    output.Write(buffer, 0, buffer.Length);

                    // Close the stream
                    output.Close();

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Something happened\n{e.Message}");
            }
        }


        /*
         * Return your entire HTML page.
         */
        protected virtual string ConstructHTML()
        {
            string contents = @"
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
        <title>Eel slap!</title>
        <meta property=""og:title"" content=""Eel slap!"" />
        <meta property=""og:type"" content=""website"" />
        <meta property=""og:url"" content=""http://www.eelslap.com"" />
        <meta property=""og:image"" content=""http://www.eelslap.com/facebook.png"" />
        <meta property=""og:site_name"" content=""Eel slap!"" />
        <meta property=""fb:admins"" content=""543574574"" />
        <META NAME=""keywords"" CONTENT=""eel slap, eelslap, eel slapping, eelslapping, eel, eal, eal slapping, eal slap, fish slapping, fishslapping,  slap this guy with an eel, slap a guy with an eel slap a man with an eel, slap a dude with an eel, slap a guy with eel, slap a man with eel, slap a dude with eel, slap someone with an eel, slap someone with eel, eal slap, ell slap, per hansson, per stenius, fimpen"">
        <META NAME=""description"" CONTENT=""Ever wanted to slap someone in the face with an eel? Well, today is your lucky day."">
        <META NAME=""author"" CONTENT=""Per Stenius - http://www.perstenius.com"">
        <link rel=""SHORTCUT ICON"" href=""http://eelslap.com/favicon.ico""/>
        <script type=""text/javascript"">
            var _gaq = _gaq || [];
            _gaq.push(['_setAccount', 'UA-114693-12']);
            _gaq.push(['_trackPageview']);

            (function() {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
            })();
        </script>
        <link rel=""stylesheet"" href=""http://eelslap.com/css/normalize.css"" type=""text/css"">
        <link rel=""stylesheet"" type=""text/css"" href=""http://eelslap.com/css/eelslap.css""/>
        <script src=""https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js""></script>
        <script type=""text/javascript"">
		(function(window, $, undefined) {
	var app = window.Eel || (window.Eel = {});
	var $window = $(window);

	var currentPosition = 0;
	var targetPosition = 0;
	var browserWidth = 0;

	var loadedImages = 0;

	var autorun = function() {

		$(""#eelimage1"").one('load', function() {
			imageLoaded();
		}).each(function() {
			if(this.complete) $(this).load();
		});

		$(""#eelimage2"").one('load', function() {
			imageLoaded();
		}).each(function() {
			if(this.complete) $(this).load();
		});

		$(""#eelimage3"").one('load', function() {
			imageLoaded();
		}).each(function() {
			if(this.complete) $(this).load();
		});

		$(""#eelimage4"").one('load', function() {
			imageLoaded();
		}).each(function() {
			if(this.complete) $(this).load();
		});

		$(""#eelimage1"").attr(""src"", ""http://eelslap.com/images/eelslap_site_panorama1.jpg"");
		$(""#eelimage2"").attr(""src"", ""http://eelslap.com/images/eelslap_site_panorama2.jpg"");
		$(""#eelimage3"").attr(""src"", ""http://eelslap.com/images/eelslap_site_panorama3.jpg"");
		$(""#eelimage4"").attr(""src"", ""http://eelslap.com/images/eelslap_site_panorama4.jpg"");
		//$(""#eelimage3"").attr(""src"", ""images/new3.jpg"");

		//startSlap();
	};

	var imageLoaded = function() {
		loadedImages++;

		if (loadedImages == 4) {
			$(""#loader"").animate({ opacity: 0 }, 500, ""linear"", function() {
				$(""#loader"").css(""display"",""none"");
			});
			setTimeout(function() {
				$(""#allimages"").css(""display"",""block"");
				$(""#allimages"").animate({ opacity: 1 }, 3000, ""linear"");
				if (isTouchDevice()) {
					setTimeout(function() {
						$(""#introtext"").css(""display"",""block"");
						$(""#introtext"").html(""Drag your finger across the screen to slap!"");
						$(""#introtext"").css(""display"",""block"");
						$(""#introtext"").animate({ opacity: 1 }, 1000, ""linear"");

						setTimeout(function() {
							$(""#introtext"").animate({ opacity: 0 }, 1000, ""linear"", function() {
								$(""#introtext"").css(""display"",""none"");
							});
						}, 3000);
					}, 1000);
				}

				startSlap();
			}, 500);
		}
	};

	var startSlap = function() {
		browserWidth = $(window).width();

		setInterval(function() {
			currentPosition += (targetPosition - currentPosition) / 4;
			var currentSlap = currentPosition / 640 * 93;
			currentSlap = Math.min(93, Math.max(0,currentSlap));
			var pos = Math.round(currentSlap) * -640;

			$(""#allimages"").css(""left"", pos);
		}, 30);

		$(""body"").bind('mousemove', function(e) {
			// $('#status').html(e.pageX +', '+ e.pageY);
			targetPosition = 640 - Math.max(0, Math.min(640, e.pageX - $('#eelcontainer').offset().left));
			//targetPosition = browserWidth - (e.pageX - $('#eelcontainer').offset().left);
			// console.log(targetPosition);
			$(""#bugger"").html(targetPosition);
		});

		$(""body"").bind('touchmove', function(e) {
			e.preventDefault();
			var touch = event.targetTouches[event.targetTouches.length-1];
			$(""#bugger"").html(""TOUCH: "" + touch.pageX + "", "" + event.targetTouches.length);
			targetPosition = browserWidth - touch.pageX;
		});

		$(window).resize(function() {
			browserWidth = $(window).width();
		});
	};

	var isTouchDevice = function() {
		var el = document.createElement('div');
		el.setAttribute('ongesturestart', 'return;');
		return typeof el.ongesturestart === ""function"";
	};

	// On DOM ready
	$(autorun);

})(this, jQuery);
</script>

    </head>
    <body>
        <div id=""eelcontainer"" class=""eel"">
            <div id=""loader"">LOADING...</div>
            <div id=""introtext"">yo</div>
            <div id=""allimages"">
                <img class=""eelimages"" id=""eelimage1"" src="""" width=""15360"" height=""480"">
                <img class=""eelimages"" id=""eelimage2"" src="""" width=""14720"" height=""480"">
                <img class=""eelimages"" id=""eelimage3"" src="""" width=""15360"" height=""480"">
                <img class=""eelimages"" id=""eelimage4"" src="""" width=""14720"" height=""480"">
            </div>
        </div>
        <div class=""footer"">
            <a href=""https://twitter.com/share"" class=""twitter-share-button"" data-url=""http://www.eelslap.com"" data-count=""vertical"">Tweet</a>
            <script type=""text/javascript"" src=""http://platform.twitter.com/widgets.js"">
            </script>
            <iframe src=""http://www.facebook.com/plugins/like.php?href=http%3A%2F%2Fwww.eelslap.com&amp;send=false&amp;layout=box_count&amp;width=55&amp;show_faces=false&amp;action=like&amp;colorscheme=light&amp;font&amp;height=62"" scrolling=""no"" frameborder=""0"" style=""border:none; overflow:hidden; width:55px; height:62px;"">
            </iframe><br>

            <a href=""http://actnormal.co"" target=""_blank"">made by</a>
        </div>
    </body>
</html>

";
            return contents;
        }


        protected string GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                // If it's an IPv4 Address
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "";
        }
    }
}
