import System.Windows.Forms from System.Windows.Forms
#import System.Drawing from System.Drawing
import System.IO

fileContent = string.Empty  

f = Form()
f.FormBorderStyle = FormBorderStyle.Sizable
f.StartPosition = FormStartPosition.CenterScreen #Centralizado
f.Text = "Form Boo"
f.Name = "Form1"
f.Width = 500
f.Height = 500

# Criar the Button 1 to the form.
textbox1 = TextBox()
textbox1.Name = "txt1"
#textbox.Size = Size(120,13)
#textbox.Text = "Digite "
textbox1.TabIndex = 1
#textbox.Location = Point(160,100)
textbox1.AcceptsReturn = true;
textbox1.AcceptsTab = true;
textbox1.Dock = System.Windows.Forms.DockStyle.Fill;
textbox1.Multiline = true;
textbox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

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
        fileStream = openFileDialog1.OpenFile()
        reader = StreamReader(fileStream)
        fileContent = reader.ReadToEnd()
        textbox1.Text = fileContent

f.Menu = mainMenu1


#Adiciona os objetos ao form
f.Controls.Add(textbox1)


#Roda a aplicação.
Application.Run(f)
