﻿

#pragma checksum "C:\Users\Jiongyu\Desktop\Puzzle game - Copy\puzzleGame\puzzleGame\puzzleGame.Windows\gamePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "71A5435AD237A2FE855C67C5AA30EB52"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace puzzleGame
{
    partial class gamePage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 13 "..\..\..\gamePage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Resume_Game_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 14 "..\..\..\gamePage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Pause_Game_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 16 "..\..\..\gamePage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.Toggle_SreenLock_Checked;
                 #line default
                 #line hidden
                #line 16 "..\..\..\gamePage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Unchecked += this.Toggle_SreenLock_Unchecked;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 22 "..\..\..\gamePage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.button_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 26 "..\..\..\gamePage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Grid_OnTapped;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


