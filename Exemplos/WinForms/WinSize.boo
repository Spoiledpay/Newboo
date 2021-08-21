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
   

f = Form()
f.FormBorderStyle = FormBorderStyle.Sizable
f.StartPosition = FormStartPosition.CenterScreen #Centralizado
f.Text = "Form Boo"
f.Name = "Form1"
f.Width = 500
f.Height = 500

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
    MessageBox.Show("O botão foi clicado ")

f.Menu = mainMenu1

# Criar the Button 1 to the form.
label = Label()
label.Name = "lb1"
label.Size = Size(120,13)
label.Text = "Data atual: " + date.Now
label.TabIndex = 1
label.Location = Point(160,100)


bt2 = Button()
bt2.Name = "btn1"
bt2.Text = "Abrir"
bt2.TabIndex = 2
bt2.Location = Point(160,30) #Posição
bt2.Dock = 0 #None
bt2.Size = Size(100, 50)
bt2.Click += def: 
    MessageBox.Show("O botão foi clicado " + bt2.Location.GetType())


# Criar the Button 2 to the form.
bt = Button()
bt.Name = "btn1"
bt.Text = "Gravar"
bt.TabIndex = 3
bt.Location = Point(40,30) #Posição
bt.Dock = 0 #None
bt.Size = Size(100, 50)
bt.Click += def: 
    MessageBox.Show("O botão foi clicado " + bt.Location.GetType())

 

#Adiciona os objetos ao form
f.Controls.Add(label)
f.Controls.Add(bt2)
f.Controls.Add(bt)

#Roda a aplicação.
Application.Run(f)
