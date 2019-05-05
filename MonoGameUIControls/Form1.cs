using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using MonoGame.Forms.Components;

namespace MonoGameUIControls
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }

    public class drawTest : MonoGame.Forms.Controls.MonoGameControl
    {
        string WelcomeMessage = "Hello MonoGame.Forms!";
        //List<MGControl> UIControls;
        MGCheckBox newCheckbox;
        MGButton newButton;
        List<MGControl> controls;
        public drawTest()
        {
        }
        //Texture2D sprite;
        protected override void Initialize()
        {
            base.Initialize();
            Editor.DisplayBackColor = Microsoft.Xna.Framework.Color.White;
            //Editor.graphics.Viewport.
            //Console.WriteLine(Editor.Content.);
            Console.WriteLine(Editor.Content.ServiceProvider.ToString());
            Console.WriteLine(Editor.Content.RootDirectory.ToString());

            newButton = new MGButton(Editor.Content.Load<Texture2D>("Button"), Editor.Content.Load<SpriteFont>("Font2"))
            {
                Position = new Vector2(350, 200),
                Text = "Random",
            };
            newButton.Click += clickhappened;
            controls = new List<MGControl>();
            controls.Add(newButton);

            newCheckbox = new MGCheckBox(Editor.Content.Load<Texture2D>("CheckBoxEmpty"), Editor.Content.Load<Texture2D>("Check"), Editor.Content.Load<Texture2D>("CheckBoxBackground"), Editor.Content.Load<SpriteFont>("Font2"))
            {
                Position = new Vector2(350, 100),
                Text = "Look Ma",
            };
            newCheckbox.Click += checkHappened;
            controls.Add(newCheckbox);
        }

        protected override void Draw()
        {
            base.Draw();
            Editor.spriteBatch.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.White);
            //if(sprite == null)
            //{
            //  sprite = Editor.Content.Load<Texture2D>("Content/Button");
            //}
            Editor.spriteBatch.Begin();

            Editor.spriteBatch.DrawString(Editor.Font, WelcomeMessage, new Vector2(
                (Editor.graphics.Viewport.Width / 2) - (Editor.Font.MeasureString(WelcomeMessage).X / 2),
                (Editor.graphics.Viewport.Height / 2) - (Editor.FontHeight / 2)),
                Microsoft.Xna.Framework.Color.White);

            foreach(MGControl m in controls)
            {
                m.Draw(Editor.GameTime, Editor.spriteBatch);
            }

            Editor.spriteBatch.End();
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (MGControl m in controls)
            {
                m.Update(gameTime);
            }
        }

        public void clickhappened(object sender, EventArgs e)
        {
            newCheckbox.Enabled = !newCheckbox.Enabled;
            Console.WriteLine("Click: "+ Editor.GameTime.ElapsedGameTime.ToString());
        }

        public void checkHappened(object sender, EventArgs e)
        {
            if (((MGCheckBox)sender).Checked)
                ((MGCheckBox)sender).Text = "I dun been clicked";
            else
                ((MGCheckBox)sender).Text = "Look Ma";
        }
    }
}
