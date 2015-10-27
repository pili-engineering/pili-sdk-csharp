# Pili Streaming Cloud server-side library for csharp

## Features

- Stream Create,Get,List
    - [x] hub.createStream()
    - [x] hub.getStream()
    - [x] hub.listStreams()
- Stream operations else
    - [x] stream.toJsonString()
    - [x] stream.update()
    - [x] stream.disable()
    - [x] stream.enable()
    - [x] stream.status()
    - [x] stream.rtmpPublishUrl()
    - [x] stream.rtmpLiveUrls()
    - [x] stream.hlsLiveUrls()
    - [x] stream.httpFlvLiveUrls()
    - [x] stream.segments()
    - [x] stream.hlsPlaybackUrls()
    - [x] stream.saveAs()
    - [x] stream.snapshot()
    - [x] stream.delete()

## Contents
- [Installation](#installation)
- [Dependencies](#dependencies)
- [Runtime Requirements](#runtime-requirements)
- [Usage](#usage)
    - [Configuration](#configuration)
    - [Hub](#hub)
        - [Instantiate a Pili Hub object](#instantiate-a-pili-hub-object)
        - [Create a new Stream](#create-a-new-stream)
        - [Get an exit Stream](#get-an-exist-stream)
        - [List Streams](#list-streams)
    - [Stream](#stream)
        - [To JSON string](#to-json-string)
        - [Update a Stream](#update-a-stream)
        - [Disable a Stream](#disable-a-stream)
        - [Enable a Stream](#enable-a-stream)
        - [Get Stream status](#get-stream-status)
        - [Generate RTMP publish URL](#generate-rtmp-publish-url)
        - [Generate RTMP live play URLs](#generate-rtmp-live-play-urls)
        - [Generate HLS live play URLs](#generate-hls-play-urls)
        - [Generate Http-Flv live play URLs](#generate-http-flv-live-play-urls)
        - [Get Stream segments](#get-stream-segments)
        - [Generate HLS playback URLs](#generate-hls-playback-urls)
        - [Save Stream as a file](#save-stream-as-a-file)
        - [Snapshot Stream](#snapshot-stream)
        - [Delete a Stream](#delete-a-stream)
- [History](#history)

### Installation
可以使用nuget 搜索pilicsharp来直接安装，目前还没发布，所以不能使用。只能用源码来下载编译出dll文件使用。

### Dependencies

You also need System.Web Newtonsoft.Json

[1]: Newtonsoft.Json 通过nuget安装



### Usage

#### Configuration

```csharp
  // Replace with your keys
 public const string ACCESS_KEY = "Qiniu_AccessKey";
 public const string SECRET_KEY = "Qiniu_SecretKey";
 
  
  // Replace with your hub name
   public const string HUB_NAME = "Pili_Hub_Name";
 // The Hub must be exists before use
  
  // Change API host as necessary
  //
  // pili.qiniuapi.com as default
  // pili-lte.qiniuapi.com is the latest RC version
  //
  // static {
  //    Configuration.getInstance().setAPIHost("pili.qiniuapi.com"); // default
  // }
```

#### Hub

##### Instantiate a Pili Hub object
```csharp
  // Instantiate an Hub object
  Credentials credentials = new Credentials(ACCESS_KEY, SECRET_KEY); // Credentials Object
  Hub hub = new Hub(credentials, HUB_NAME); // Hub Object
```

##### Create a new stream

```csharp

   // Create a new Stream
   string title = null; // optional, auto-generated as default
            string publishKey = null; // optional, auto-generated as default
            string publishSecurity = null; // optional, can be "dynamic" or "static", "dynamic" as default
            Stream stream = null;
            try
            {
                stream = hub.createStream(title, publishKey, publishSecurity);
                Trace.WriteLine("hub.createStream:");
                Console.WriteLine(stream.toJsonString());
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
```

or


```csharp
  try {
    Stream stream = hub.createStream();
  } catch (PiliException e) {
    e.printStackTrace();
  }
```

##### Get an exist Stream

```csharp
  try
            {
                stream = hub.getStream(stream.StreamId);
                Console.WriteLine("hub.getStream:");
                Console.WriteLine(stream.toJsonString());
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
```

##### List Streams

```csharp
  try
            {
                string marker = null; // optional
                long limit = 0; // optional
                string titlePrefix = null; // optional

                Stream.StreamList streamList = hub.listStreams(marker, limit, titlePrefix);
                Console.WriteLine("hub.listStreams()");
                Console.WriteLine("marker:" + streamList.Marker);
                IList<Stream> list = streamList.Streams;
                foreach (Stream s in list)
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

```
or

```csharp
 try {
    pili_sdk_csharp.pili.Stream.StreamList list = hub.listStreams();
    if (list != null) {
        foreach (var s in list.Streams)
        {
            Console.WriteLine(stream.toJsonString());
        }
    }
  } catch (PiliException e)
  {

      Console.WriteLine(e.StackTrace);
  }
```
#### Stream

##### To JSON string

```csharp
			string streamJsonString = stream.toJsonString();
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
```
##### Update a Stream

```csharp
// Update a Stream
            string newPublishKey = null; // optional
            string newPublishSecurity = null; // optional, can be "dynamic" or "static"
            bool newDisabled = true; // optional, can be "true" of "false"
            try
            {
                Stream newStream = stream.update(newPublishKey, newPublishSecurity, newDisabled);
                Console.WriteLine("Stream update()");
                Console.WriteLine(newStream.toJsonString());
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
```

##### Disable a Stream

```csharp
// Disable a Stream
 try
            {
                Stream disabledStream = stream.disable();
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
```

##### Enable a Stream

```csharp
// Enable a Stream
 try
            {
                Stream enabledStream = stream.enable();
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
```

##### Get Stream status

```csharp
// Get Stream status
 try
            {
                Stream.Status status = stream.status();
                Console.WriteLine("Stream status()");
                Console.WriteLine(status.ToString());
                /*
                {
                    "addr":"222.73.202.226:2572",
                    "status":"disconnected",
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
```

##### Generate RTMP publish URL

```csharp
// Generate RTMP publish URL
 try
            {
                string publishUrl = stream.rtmpPublishUrl();
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
```

##### Generate RTMP live play URLs

```csharp
// Generate RTMP live play URLs
            string originUrl = (stream.rtmpLiveUrls())[Stream.ORIGIN];
            Console.WriteLine("Stream rtmpLiveUrls()");
            Console.WriteLine(originUrl);
            // rtmp://ey636h.live1-rtmp.z1.pili.qiniucdn.com/test-hub/55d8113ee3ba5723280000dc
```

##### Generate HLS live play URLs

```csharp
// Generate HLS play URLs
            string originLiveHlsUrl = stream.hlsLiveUrls()[Stream.ORIGIN];
            Console.WriteLine("Stream hlsLiveUrls()");
            Console.WriteLine(originLiveHlsUrl);
            // http://ey636h.live1-http.z1.pili.qiniucdn.com/test-hub/55d8119ee3ba5723280000dd.m3u8
```

##### Generate Http Flv live play URLs

```csharp
// Generate Http-Flv live play URLs
            string originLiveFlvUrl = stream.httpFlvLiveUrls()[Stream.ORIGIN];
            Console.WriteLine("Stream httpFlvLiveUrls()");
            Console.WriteLine(originLiveFlvUrl);
            // http://ey636h.live1-http.z1.pili.qiniucdn.com/test-hub/55d8119ee3ba5723280000dd.flv
```

##### Get Stream segments

```csharp
// Get Stream segments
   try
            {
                long start = 0; // optional, in second, unix timestamp
                long end = 0; // optional, in second, unix timestamp
                int limit = 0; // optional, int
                Stream.SegmentList segmentList = stream.segments(start, end, limit);

                Console.WriteLine("Stream segments()");
                foreach (Stream.Segment segment in segmentList.getSegmentList())
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
```

##### Generate HLS playback URLs

```csharp
// Generate HLS playback URLs
            long startHlsPlayback = 1440315411; // required, in second, unix timestamp
            long endHlsPlayback = 1440315435; // required, in second, unix timestamp
            try
            {
                string hlsPlaybackUrl = stream.hlsPlaybackUrls(startHlsPlayback, endHlsPlayback)[Stream.ORIGIN];

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

```

##### Save Stream as a file

```csharp
// Save Stream as a file
            string saveAsFormat = "mp4"; // required
			string saveAsName = "videoName" + "." + saveAsFormat; // required
            long saveAsStart = 1444897613; // required, in second, unix timestampstart:1444897613,end:1444897973
            long saveAsEnd = 1444897973; // required, in second, unix timestamp
			string saveAsNotifyUrl = null; // optional
			try
			{
				Stream.SaveAsResponse response = stream.saveAs(saveAsName, saveAsFormat, saveAsStart, saveAsEnd, saveAsNotifyUrl);
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
```

While invoking `saveAs()` and `snapshot()`, you can get processing state via Qiniu fop service using `persistentId`.   
API: `curl -D GET http://api.qiniu.com/status/get/prefop?id={persistentId}`  
Doc reference: <http://developer.qiniu.com/docs/v6/api/overview/fop/persistent-fop.html#pfop-status>  

##### Snapshot Stream

```csharp
// Snapshot Stream
            string format = "jpg"; // required
            string name = "imageName" + "." + format; // required
            long time = 1440315411; // optional, in second, unix timestamp
            string notifyUrl = null; // optional

            try
            {
                Stream.SnapshotResponse response = stream.snapshot(name, format, time, notifyUrl);
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
```

##### Delete a stream

```csharp
// Delete a Stream
 try
            {
                string res = stream.delete();
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
```

## History
- 1.5.0
  - Add Stream Create,Get,List
    - hub.createStream()
    - hub.getStream()
    - hub.listStreams()
  - Add Stream operations else
    - stream.toJsonString()
    - stream.update()
    - stream.disable()
    - stream.enable()
    - stream.status()
    - stream.segments()
    - stream.rtmpPublishUrl()
    - stream.rtmpLiveUrls()
    - stream.hlsLiveUrls()
    - stream.httpFlvLiveUrls()
    - stream.hlsPlaybackUrls()
    - stream.snapshot()
    - stream.saveAs()
    - stream.delete()

