﻿#pragma checksum "C:\Repos\DesktopApplication\VerbApplication\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "471C4A1D641B46B08EE0CF9C04A8470A1BF7CAE4C558500FBBE3AE34468FF470"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VerbApplication
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 17
                {
                    this.RemoteVideo = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                }
                break;
            case 3: // MainPage.xaml line 18
                {
                    this.LocalVideo = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                }
                break;
            case 4: // MainPage.xaml line 12
                {
                    this.CalleeTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // MainPage.xaml line 13
                {
                    this.CallButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.CallButton).Click += this.CallButton_ClickAsync;
                }
                break;
            case 6: // MainPage.xaml line 14
                {
                    this.HangupButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.HangupButton).Click += this.HangupButton_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
