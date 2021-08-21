#region license
// Copyright (c) 2021, Fernando Medeiros Dantas (fernando@spoiledpay.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Fernando Medeiros Dantas nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

import System.Windows.Forms from System.Windows.Forms
import System.Drawing from System.Drawing
import System.IO

#fileContent = string.Empty
#filePath = string.Empty

f = Form()
f.FormBorderStyle = FormBorderStyle.Sizable
f.StartPosition = FormStartPosition.CenterScreen #Centralizado
f.Text = "Hello World"
f.Name = "Form1"
f.Height = 200
f.Width = 300

# Create a main menu object.
mainMenu1 =  MainMenu()

// Create empty menu item objects.
topMenuItem = MenuItem()
menuItem1 = MenuItem()

// Set the caption of the menu items.
topMenuItem.Text = "&Arquivo"
menuItem1.Text = "&Abrir"

// Add the menu items to the main menu.
topMenuItem.MenuItems.Add(menuItem1);
mainMenu1.MenuItems.Add(topMenuItem);

menuItem1.Click += def: 
    openFileDialog1 = OpenFileDialog()
    openFileDialog1.Title = "Browse Text Files"
    openFileDialog1.InitialDirectory = "c:\\"
    openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
    openFileDialog1.FilterIndex = 2
    openFileDialog1.RestoreDirectory = true
    openFileDialog1.ShowReadOnly = true
    if openFileDialog1.ShowDialog() == DialogResult.OK:
        MessageBox.Show("Arquivo selecionado: " + openFileDialog1.FileName)
    
    //Get the path of specified file
   # filePath = openFileDialog1.FileName

    //Read the contents of the file into a stream
    #fileStream = openFileDialog1.OpenFile()

f.Menu = mainMenu1
Application.Run(f)