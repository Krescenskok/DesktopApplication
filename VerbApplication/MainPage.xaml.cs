using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Azure.WinRT.Communication;
using Azure.Communication.Calling;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VerbApplication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.InitCallAgentAndDeviceManager();
        }

        private async void InitCallAgentAndDeviceManager()
        {
            CallClient callClient = new CallClient();
            deviceManager = await callClient.GetDeviceManager();

            CommunicationTokenCredential token_credential = new CommunicationTokenCredential("eyJhbGciOiJSUzI1NiIsImtpZCI6IjEwMyIsIng1dCI6Ikc5WVVVTFMwdlpLQTJUNjFGM1dzYWdCdmFMbyIsInR5cCI6IkpXVCJ9.eyJza3lwZWlkIjoiYWNzOjIwOGYzOTczLTQ1NjItNDRlYS04NTI5LWUyNjE2MDIwOGM5Ml8wMDAwMDAwZC0xYTczLTY0ZjYtODVkOS1hNTNhMGQwMDFmOTQiLCJzY3AiOjE3OTIsImNzaSI6IjE2MzQxNDM5NDUiLCJleHAiOjE2MzQyMzAzNDUsImFjc1Njb3BlIjoidm9pcCIsInJlc291cmNlSWQiOiIyMDhmMzk3My00NTYyLTQ0ZWEtODUyOS1lMjYxNjAyMDhjOTIiLCJpYXQiOjE2MzQxNDM5NDV9.AgV9aEbg62gXV7-JPI6wwzVUxszgz08VOD-pF1xKJfSJlKde5Qi4zP5w7vib0Aifvi4OUbc9w1rRxQ4oahJrl2sag5T5VP_QCPqf3pcD63E0bqIiNF74UXKcGpoXcXwf6haE_1FHr6R9iHTnwxXxAX2CKQ75yNH186tfAOxbJnD66StSxnoZeSjh2FJ7DBUAw7QvV2KE3o1u4DJT77kYYYjlkeJ6JlXVZ0z0NAwmKJMaLHKQMQKO0Aunln3MXrgDkdj7euAHJB_c515F7jeIANYPYt3FlrnEGg1njOIqw9lPxmih5b6Vpcl-df_-WgvPwrZeRmPk0SwPvBAIKW4LEQ");

            CallAgentOptions callAgentOptions = new CallAgentOptions()
            {
                DisplayName = "Host"
            };
            callAgent = await callClient.CreateCallAgent(token_credential, callAgentOptions);
            callAgent.OnCallsUpdated += Agent_OnCallsUpdated;
            callAgent.OnIncomingCall += Agent_OnIncomingCall;
        }

        private async void CallButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            Debug.Assert(deviceManager.Microphones.Count > 0);
            Debug.Assert(deviceManager.Speakers.Count > 0);
            Debug.Assert(deviceManager.Cameras.Count > 0);

            if (deviceManager.Cameras.Count > 0)
            {
                VideoDeviceInfo videoDeviceInfo = deviceManager.Cameras[0];
                localVideoStream = new LocalVideoStream[1];
                localVideoStream[0] = new LocalVideoStream(videoDeviceInfo);

                Uri localUri = await localVideoStream[0].CreateBindingAsync();

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    LocalVideo.Source = localUri;
                    LocalVideo.Play();
                });

            }


            JoinCallOptions joinCallOptions = new JoinCallOptions();
            var videoOptions = new VideoOptions(localVideoStream);
            joinCallOptions.VideoOptions = videoOptions;
            Debug.WriteLine(joinCallOptions.VideoOptions.LocalVideoStreams.Count);
            var groupId = Guid.Parse("936fae55-67fe-4268-8b2a-e68bc6062a00");
            var groupLocator = new GroupCallLocator(groupId);
            call = await callAgent.JoinAsync(groupLocator, joinCallOptions);
            Debug.WriteLine(call.Id);
        }

        private async void Agent_OnIncomingCall(object sender, IncomingCall incomingcall)
        {
            Debug.Assert(deviceManager.Microphones.Count > 0);
            Debug.Assert(deviceManager.Speakers.Count > 0);
            Debug.Assert(deviceManager.Cameras.Count > 0);

            if (deviceManager.Cameras.Count > 0)
            {
                VideoDeviceInfo videoDeviceInfo = deviceManager.Cameras[0];
                localVideoStream = new LocalVideoStream[1];
                localVideoStream[0] = new LocalVideoStream(videoDeviceInfo);

                Uri localUri = await localVideoStream[0].CreateBindingAsync();

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    LocalVideo.Source = localUri;
                    LocalVideo.Play();
                });

            }
            AcceptCallOptions acceptCallOptions = new AcceptCallOptions();
            acceptCallOptions.VideoOptions = new VideoOptions(localVideoStream);

            call = await incomingcall.AcceptAsync(acceptCallOptions);
        }

        private async void Agent_OnCallsUpdated(object sender, CallsUpdatedEventArgs args)
        {
            foreach (var call in args.AddedCalls)
            {
                foreach (var remoteParticipant in call.RemoteParticipants)
                {
                    await AddVideoStreams(remoteParticipant.VideoStreams);
                    remoteParticipant.OnVideoStreamsUpdated += async (s, a) => await AddVideoStreams(a.AddedRemoteVideoStreams);
                }
                call.OnRemoteParticipantsUpdated += Call_OnRemoteParticipantsUpdated;
                call.OnStateChanged += Call_OnStateChanged;
            }
        }

        private async void Call_OnRemoteParticipantsUpdated(object sender, ParticipantsUpdatedEventArgs args)
        {
            foreach (var remoteParticipant in args.AddedParticipants)
            {
                await AddVideoStreams(remoteParticipant.VideoStreams);
                remoteParticipant.OnVideoStreamsUpdated += async (s, a) => await AddVideoStreams(a.AddedRemoteVideoStreams);
            }
        }

        private async Task AddVideoStreams(IReadOnlyList<RemoteVideoStream> streams)
        {

            foreach (var remoteVideoStream in streams)
            {
                var remoteUri = await remoteVideoStream.CreateBindingAsync();

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    RemoteVideo.Source = remoteUri;
                    RemoteVideo.Play();
                });
                remoteVideoStream.Start();
            }
        }

        private async void Call_OnStateChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (((Call)sender).State)
            {
                case CallState.Disconnected:
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        LocalVideo.Source = null;
                        RemoteVideo = null;
                    });
                    break;
                default:
                    Debug.WriteLine(((Call)sender).State);
                    break;
            }
        }

        private async void HangupButton_Click(object sender, RoutedEventArgs e)
        {
            var hangUpOptions = new HangUpOptions();
            await call.HangUpAsync(hangUpOptions);
            Debug.WriteLine("call has ended");
        }

        
        CallClient callClient;
        CallAgent callAgent;
        Call call;
        DeviceManager deviceManager;
        LocalVideoStream[] localVideoStream;
    }
}