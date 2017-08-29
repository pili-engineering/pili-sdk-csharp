using System;
using System.Diagnostics;
using pili_sdk_csharp.pili;
using pili_sdk_csharp.pili_qiniu;

namespace pili_sdk_csharp_example
{
    internal class Example
    {
        // Replace with your keys here
        private const string AccessKey = "";

        private const string SecretKey = "";

        // Replace with your hub name
        private const string HubName = "";

        // Change API host as necessary
        //
        // pili.qiniuapi.com as default
        // pili-lte.qiniuapi.com is the latest RC version
        //
        // static {
        //     Configuration.getInstance().setAPIHost("pili.qiniuapi.com"); // default
        // }
        private static void Main(string[] args)
        {
            TestCreatStream();
            //测试推流后才能执行操作的函数，需要填写生成的流的id。
            // testTuiStream("z1.liuhanlin.561f62c5fb16df53010003ed");
            Console.ReadKey();
        }

        public static void TestStream(string streamid)
        {
            Stream stream = null;
            var credentials = new Credentials(AccessKey, SecretKey); // Credentials Object
            var hub = new Hub(credentials, HubName);
            //get a stream
            try
            {
                stream = hub.GetStream(streamid);
                Console.WriteLine("hub.getStream:");
                Console.WriteLine(stream.ToJsonString());
                /*
                 {
                      "id": "z1.liuhanlin.562f2b35d409d2aa48001102",
                      "createdAt": "2015-10-27T07:43:49.756Z",
                      "updatedAt": "2015-10-27T07:43:49.756Z",
                      "title": "562f2b35d409d2aa48001102",
                      "hub": "liuhanlin",
                      "disabled": false,
                      "publishKey": "9654c11a01b7b941",
                      "publishSecurity": "static",
                      "hosts": {
                        "publish": {
                          "rtmp": "100000p.publish.z1.pili.qiniup.com"
                        },
                        "live": {
                          "hdl": "100000p.live1-hdl.z1.pili.qiniucdn.com",
                          "hls": "100000p.live1-hls.z1.pili.qiniucdn.com",
                          "http": "100000p.live1-hls.z1.pili.qiniucdn.com",
                          "rtmp": "100000p.live1-rtmp.z1.pili.qiniucdn.com"
                        },
                        "playback": {
                          "hls": "100000p.playback1.z1.pili.qiniucdn.com",
                          "http": "100000p.playback1.z1.pili.qiniucdn.com"
                        },
                        "play": {
                          "http": "100000p.live1-hls.z1.pili.qiniucdn.com",
                          "rtmp": "100000p.live1-rtmp.z1.pili.qiniucdn.com"
                        }
                      }
                    }
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            //// Get Stream segments
            try
            {
                long start = 0; // optional, in second, unix timestamp
                long end = 0; // optional, in second, unix timestamp
                var limit = 0; // optional, int
                var segmentList = stream.Segments(start, end, limit);

                Console.WriteLine("Stream segments()");
                foreach (var segment in segmentList.GetSegmentList())
                {
                    Console.WriteLine("start:" + segment.Start + ",end:" + segment.End);
                }
                /*
                     start:1440315411,end:1440315435
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            // Snapshot Stream
            var format = "jpg"; // required
            var name = "imageName" + "." + format; // required
            long time = 1440315411; // optional, in second, unix timestamp
            string notifyUrl = null; // optional

            try
            {
                var response = stream.Snapshot(name, format, time, notifyUrl);
                Console.WriteLine("Stream snapshot()");
                Console.WriteLine(response.ToString());
                /*
                 {
                     "targetUrl":"http://ey636h.static1.z0.pili.qiniucdn.com/snapshots/z1.test-hub.55d81a72e3ba5723280000ec/imageName.jpg",
                     "persistentId":"z1.55d81c247823de5a49ad729c"
                 }
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            // Save Stream as a file
            var saveAsFormat = "mp4"; // required
            var saveAsName = "videoName" + "." + saveAsFormat; // required
            long saveAsStart = 1444897613; // required, in second, unix timestampstart:1444897613,end:1444897973
            long saveAsEnd = 1444897973; // required, in second, unix timestamp
            string saveAsNotifyUrl = null; // optional
            string pipleline = null;
            try
            {
                var response = stream.SaveAs(saveAsName, saveAsFormat, saveAsStart, saveAsEnd, saveAsNotifyUrl, pipleline);
                Console.WriteLine("Stream saveAs()");
                Console.WriteLine(response.ToString());
                /*
                 {
                     "url":"http://ey636h.vod1.z1.pili.qiniucdn.com/recordings/z1.test-hub.55d81a72e3ba5723280000ec/videoName.m3u8",
                     "targetUrl":"http://ey636h.vod1.z1.pili.qiniucdn.com/recordings/z1.test-hub.55d81a72e3ba5723280000ec/videoName.mp4",
                     "persistentId":"z1.55d81c6c7823de5a49ad77b3"
                 }
                */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            // Delete a Stream
            try
            {
                var res = stream.Delete();
                Console.WriteLine("Stream delete()");
                Console.WriteLine(res);
                // No Content
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            //////////////////////////////////////////////////////////////////////////////////////////
            // Stream end

            //get a stream
            try
            {
                stream = hub.GetStream(streamid);
                Console.WriteLine("hub.getStream:");
                Console.WriteLine(stream.ToJsonString());
                /*
                   {
                      "id": "z1.liuhanlin.562f2b35d409d2aa48001102",
                      "createdAt": "2015-10-27T07:43:49.756Z",
                      "updatedAt": "2015-10-27T07:43:49.756Z",
                      "title": "562f2b35d409d2aa48001102",
                      "hub": "liuhanlin",
                      "disabled": false,
                      "publishKey": "9654c11a01b7b941",
                      "publishSecurity": "static",
                      "hosts": {
                        "publish": {
                          "rtmp": "100000p.publish.z1.pili.qiniup.com"
                        },
                        "live": {
                          "hdl": "100000p.live1-hdl.z1.pili.qiniucdn.com",
                          "hls": "100000p.live1-hls.z1.pili.qiniucdn.com",
                          "http": "100000p.live1-hls.z1.pili.qiniucdn.com",
                          "rtmp": "100000p.live1-rtmp.z1.pili.qiniucdn.com"
                        },
                        "playback": {
                          "hls": "100000p.playback1.z1.pili.qiniucdn.com",
                          "http": "100000p.playback1.z1.pili.qiniucdn.com"
                        },
                        "play": {
                          "http": "100000p.live1-hls.z1.pili.qiniucdn.com",
                          "rtmp": "100000p.live1-rtmp.z1.pili.qiniucdn.com"
                        }
                      }
                    }
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        public static void TestCreatStream()
        {
            //////////////////////////////////////////////////////////////////////////////////////////
            // Hub begin
            //////////////////////////////////////////////////////////////////////////////////////////

            // Instantiate an Hub object
            var credentials = new Credentials(AccessKey, SecretKey); // Credentials Object
            var hub = new Hub(credentials, HubName);
            Console.WriteLine(hub.ToString());

            // Create a new Stream
            string title = null; // optional, auto-generated as default
            string publishKey = null; // optional, auto-generated as default
            string publishSecurity = null; // optional, can be "dynamic" or "static", "dynamic" as default
            Stream stream = null;
            try
            {
                stream = hub.CreateStream(title, publishKey, publishSecurity);
                Trace.WriteLine("hub.createStream:");
                Console.WriteLine(stream.ToJsonString());
                /*
                {
                    "id":"z1.test-hub.55d97350eb6f92638c00000a",
                    "createdAt":"2015-08-22T04:54:13.539Z",
                    "updatedAt":"2015-08-22T04:54:13.539Z",
                    "title":"55d97350eb6f92638c00000a",
                    "hub":"test-hub",
                    "disabled":false,
                    "publishKey":"ca11e07f094c3a6e",
                    "publishSecurity":"dynamic",
                    "hosts":{
                        "publish":{
                            "rtmp":"ey636h.publish.z1.pili.qiniup.com"
                         },
                         "live":{
                             "http":"ey636h.live1-http.z1.pili.qiniucdn.com",
                             "rtmp":"ey636h.live1-rtmp.z1.pili.qiniucdn.com"
                         },
                         "playback":{
                             "http":"ey636h.playback1.z1.pili.qiniucdn.com"
                         }
                     }
                 }
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            try
            {
                stream = hub.GetStream(stream.StreamId);
                Console.WriteLine("hub.getStream:");
                Console.WriteLine(stream.ToJsonString());
                /*
                {
                    "id":"z1.test-hub.55d80075e3ba5723280000d2",
                    "createdAt":"2015-08-22T04:54:13.539Z",
                    "updatedAt":"2015-08-22T04:54:13.539Z",
                    "title":"55d80075e3ba5723280000d2",
                    "hub":"test-hub",
                    "disabled":false,
                    "publishKey":"ca11e07f094c3a6e",
                    "publishSecurity":"dynamic",
                    "hosts":{
                        "publish":{
                            "rtmp":"ey636h.publish.z1.pili.qiniup.com"
                         },
                         "live":{
                             "http":"ey636h.live1-http.z1.pili.qiniucdn.com",
                             "rtmp":"ey636h.live1-rtmp.z1.pili.qiniucdn.com"
                         },
                         "playback":{
                             "http":"ey636h.playback1.z1.pili.qiniucdn.com"
                         }
                     }
                 }
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            // List streams
            try
            {
                string marker = null; // optional
                long limit = 0; // optional
                string titlePrefix = null; // optional

                var streamList = hub.ListStreams(marker, limit, titlePrefix);
                Console.WriteLine("hub.listStreams()");
                Console.WriteLine("marker:" + streamList.Marker);
                var list = streamList.Streams;
                foreach (var s in list)
                {
                    // access the stream
                }

                /*
                 marker:10
                 stream object
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            //////////////////////////////////////////////////////////////////////////////////////////
            // Hub end
            //////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////
            // Stream begin
            //////////////////////////////////////////////////////////////////////////////////////////
            var streamJsonString = stream.ToJsonString();
            Console.WriteLine("Stream toJSONString()");
            Console.WriteLine(streamJsonString);

            /*
                {
                    "id":"z1.test-hub.55d80075e3ba5723280000d2",
                    "createdAt":"2015-08-22T04:54:13.539Z",
                    "updatedAt":"2015-08-22T04:54:13.539Z",
                    "title":"55d80075e3ba5723280000d2",
                    "hub":"test-hub",
                    "disabled":false,
                    "publishKey":"ca11e07f094c3a6e",
                    "publishSecurity":"dynamic",
                    "hosts":{
                        "publish":{
                            "rtmp":"ey636h.publish.z1.pili.qiniup.com"
                         },
                         "live":{
                             "http":"ey636h.live1-http.z1.pili.qiniucdn.com",
                             "rtmp":"ey636h.live1-rtmp.z1.pili.qiniucdn.com"
                         },
                         "playback":{
                             "http":"ey636h.playback1.z1.pili.qiniucdn.com"
                         }
                     }
                 }
             */

            // Update a Stream
            var newPublishKey = "new_secret_words"; // optional
            var newPublishSecurity = "static"; // optional, can be "dynamic" or "static"
            var newDisabled = true; // optional, can be "true" of "false"
            try
            {
                var newStream = stream.Update(newPublishKey, newPublishSecurity, newDisabled);
                Console.WriteLine("Stream update()");
                Console.WriteLine(newStream.ToJsonString());
                stream = newStream;
                /*
                {
                    "id":"z1.test-hub.55d80075e3ba5723280000d2",
                    "createdAt":"2015-08-22T04:54:13.539Z",
                    "updatedAt":"2015-08-22T01:53:02.738973745-04:00",
                    "title":"55d80075e3ba5723280000d2",
                    "hub":"test-hub",
                    "disabled":true,
                    "publishKey":"new_secret_words",
                    "publishSecurity":"static",
                    "hosts":{
                        "publish":{
                            "rtmp":"ey636h.publish.z1.pili.qiniup.com"
                         },
                         "live":{
                             "http":"ey636h.live1-http.z1.pili.qiniucdn.com",
                             "rtmp":"ey636h.live1-rtmp.z1.pili.qiniucdn.com"
                         },
                         "playback":{
                             "http":"ey636h.hls.z1.pili.qiniucdn.com"
                         }
                     }
                 }
             */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            // Disable a Stream
            try
            {
                var disabledStream = stream.Disable();
                Console.WriteLine("Stream disable()");
                Console.WriteLine(disabledStream.Disabled);
                /*
                 * true
                 * 
                 * */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            // Enable a Stream
            try
            {
                var enabledStream = stream.Enable();
                Console.WriteLine("Stream enable()");
                Console.WriteLine(enabledStream.Disabled);
                /*
                 * false
                 * 
                 * */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            // Get Stream CurrentStatus
            try
            {
                var status = stream.CurrentStatus();
                Console.WriteLine("Stream CurrentStatus()");
                Console.WriteLine(status.ToString());
                /*
                {
                    "addr":"222.73.202.226:2572",
                    "CurrentStatus":"disconnected",
                    "bytesPerSecond":0,
                    "framesPerSecond":{
                        "audio":0,
                        "video":0,
                        "data":0
                     }
                 }
                */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }

            // Generate RTMP publish URL
            try
            {
                var publishUrl = stream.RtmpPublishUrl();
                Console.WriteLine("Stream rtmpPublishUrl()");
                Console.WriteLine(publishUrl);
                // rtmp://ey636h.publish.z1.pili.qiniup.com/test-hub/55d810aae3ba5723280000db?nonce=1440223404&token=hIVJje0ZOX9hp7yPIvGBmJ_6Qxo=
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            // Generate RTMP live play URLs
            var originUrl = stream.RtmpLiveUrls()[Stream.Origin];
            Console.WriteLine("Stream rtmpLiveUrls()");
            Console.WriteLine(originUrl);
            // rtmp://ey636h.live1-rtmp.z1.pili.qiniucdn.com/test-hub/55d8113ee3ba5723280000dc

            // Generate HLS play URLs
            var originLiveHlsUrl = stream.HlsLiveUrls()[Stream.Origin];
            Console.WriteLine("Stream hlsLiveUrls()");
            Console.WriteLine(originLiveHlsUrl);
            // http://ey636h.live1-http.z1.pili.qiniucdn.com/test-hub/55d8119ee3ba5723280000dd.m3u8

            // Generate Http-Flv live play URLs
            var originLiveFlvUrl = stream.HttpFlvLiveUrls()[Stream.Origin];
            Console.WriteLine("Stream httpFlvLiveUrls()");
            Console.WriteLine(originLiveFlvUrl);
            // http://ey636h.live1-http.z1.pili.qiniucdn.com/test-hub/55d8119ee3ba5723280000dd.flv


            // Generate HLS playback URLs
            long startHlsPlayback = 1440315411; // required, in second, unix timestamp
            long endHlsPlayback = 1440315435; // required, in second, unix timestamp
            try
            {
                var hlsPlaybackUrl = stream.HlsPlaybackUrls(startHlsPlayback, endHlsPlayback)[Stream.Origin];

                Console.WriteLine("Stream hlsPlaybackUrls()");
                Console.WriteLine(hlsPlaybackUrl);
                // http://ey636h.playback1.z1.pili.qiniucdn.com/test-hub/55d8119ee3ba5723280000dd.m3u8?start=1440315411&end=1440315435
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }


            // Get Stream CurrentStatus
            try
            {
                var status = stream.CurrentStatus();
                Console.WriteLine("Stream CurrentStatus()");
                Console.WriteLine(status.ToString());
                /*
                {
                    "addr":"222.73.202.226:2572",
                    "CurrentStatus":"disconnected",
                    "bytesPerSecond":0,
                    "framesPerSecond":{
                        "audio":0,
                        "video":0,
                        "data":0
                     }
                 }
                */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }


            // Delete a Stream
            try
            {
                var res = stream.Delete();
                Console.WriteLine("Stream delete()");
                Console.WriteLine(res);
                // No Content
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            //////////////////////////////////////////////////////////////////////////////////////////
            // Stream end

            try
            {
                stream = hub.GetStream(stream.StreamId);
                Console.WriteLine("hub.getStream:");
                Console.WriteLine(stream.ToJsonString());
                /*
                {
                    "id":"z1.test-hub.55d80075e3ba5723280000d2",
                    "createdAt":"2015-08-22T04:54:13.539Z",
                    "updatedAt":"2015-08-22T04:54:13.539Z",
                    "title":"55d80075e3ba5723280000d2",
                    "hub":"test-hub",
                    "disabled":false,
                    "publishKey":"ca11e07f094c3a6e",
                    "publishSecurity":"dynamic",
                    "hosts":{
                        "publish":{
                            "rtmp":"ey636h.publish.z1.pili.qiniup.com"
                         },
                         "live":{
                             "http":"ey636h.live1-http.z1.pili.qiniucdn.com",
                             "rtmp":"ey636h.live1-rtmp.z1.pili.qiniucdn.com"
                         },
                         "playback":{
                             "http":"ey636h.playback1.z1.pili.qiniucdn.com"
                         }
                     }
                 }
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }
    }
}
