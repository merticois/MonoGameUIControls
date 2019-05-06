using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameUIControls
{
    public abstract class MGControl
    {
        public bool canBeParent
        {
            get;
            protected set;
        }

        public Vector2 Position { get; set; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }

    public class MGButton : MGControl
    {
        #region Fields
        private MouseState _currentMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _texture;
        private MGControl _parent;
        #endregion

        #region Properties
        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        

        public bool Enabled { get; set; }

        public MGControl Parent
        {
            get
            {
                if (_parent != null)
                    return _parent;
                else
                    return null;
            }
            set
            {
                if(value.canBeParent)
                {
                    _parent = value;
                }
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                Vector2 parentoffset = new Vector2(0, 0);
                if (Parent != null)
                {
                    parentoffset = Parent.Position;
                }
                return new Rectangle((int)(Position.X + parentoffset.X), (int)(Position.Y + parentoffset.Y), _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }
        #endregion

        #region Methods

        public MGButton(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            
            _font = font;
            Enabled = true;
            PenColour = Color.Black;
            canBeParent = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 parentoffset = new Vector2(0, 0);
            if (Parent != null)
            {
                parentoffset = Parent.Position;
            }
            var colour = Color.White;

            if (Enabled == false)
                colour = Color.Gray;
            else if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed && Enabled)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion
    }

    public class MGCheckBox : MGControl
    {
        #region Fields
        private MouseState _currentMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _check;
        private Texture2D _box;
        private Texture2D _texture;
        private MGControl _parent;
        #endregion

        #region Properties
        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public bool Checked { get; set; }

        public bool Enabled { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                Vector2 parentoffset = new Vector2(0, 0);
                if (Parent != null)
                {
                    parentoffset = Parent.Position;
                }
                return new Rectangle((int)(Position.X + parentoffset.X), (int)(Position.Y + parentoffset.Y), _texture.Width, _texture.Height);
            }
        }

        public MGControl Parent
        {
            get
            {
                if (_parent != null)
                    return _parent;
                else
                    return null;
            }
            set
            {
                if (value.canBeParent)
                {
                    _parent = value;
                }
            }
        }

        public Rectangle CheckBoxBounds
        {
            get
            {
                Vector2 parentoffset = new Vector2(0, 0);
                if (Parent != null)
                {
                    parentoffset = Parent.Position;
                }
                if (Text != null)
                {
                    return new Rectangle((int)(Position.X + parentoffset.X), (int)(Position.Y + parentoffset.Y), _texture.Width + 2 + (int)_font.MeasureString(Text).X, _texture.Height);
                }
                else
                {
                    return new Rectangle((int)(Position.X + parentoffset.X), (int)(Position.Y + parentoffset.Y), _texture.Width, _texture.Height);
                }
            }
        }

        public string Text { get; set; }
        #endregion

        #region Methods

        public MGCheckBox(Texture2D box, Texture2D check, Texture2D texture, SpriteFont font)
        {
            _box = box;
            _check = check;
            _texture = texture;
            _font = font;
            Checked = false;
            Enabled = true;
            PenColour = Color.Black;
            canBeParent = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.Black;
            Vector2 parentoffset = new Vector2(0,0);
            if (Parent != null)
            {
                parentoffset = Parent.Position;
            }


            if (Enabled == false)
                colour = Color.Gray;
            else if (_isHovering)
                colour = new Color(0,120,215,255);

            spriteBatch.Draw(_texture, Rectangle, Color.White);

            spriteBatch.Draw(_box, Rectangle, colour);

            if(Checked)
                spriteBatch.Draw(_check, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + Rectangle.Width + 2);
                var y = (Rectangle.Y + Rectangle.Height / 2) - (_font.MeasureString(Text).Y / 2)+1;

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(CheckBoxBounds))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed && Enabled)
                {
                    Checked = !Checked;
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion
    }

    public class MGTextBox : MGControl
    {
        public MGTextBox()
        {
            canBeParent = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }

    public class MGPanel : MGControl
    {
        #region Fields
        private MouseState _currentMouse;
        private SpriteFont _font;
        //private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _texture;
        private MGControl _parent;
        private Point _size;
        private bool _Outlined;
        private bool _Draggable;
        private int _outlineThickness;
        #endregion

        #region Properties
        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Color BackColour { get; set; }

        public bool Enabled { get; set; }

        public int OutlineThickness
        {
            get
            {
                return _outlineThickness;
            }
            set
            {
                _outlineThickness = Math.Min(Math.Max(value, 1), 5);
            }
        }

        public MGControl Parent
        {
            get
            {
                if (_parent != null)
                    return _parent;
                else
                    return null;
            }
            set
            {
                if (value.canBeParent)
                {
                    _parent = value;
                }
            }
        }

        public int Width
        {
            set
            {
                _size.X = value;
            }
            get
            {
                return _size.X;
            }
        }

        public int Height
        {
            set
            {
                _size.Y = value;
            }
            get
            {
                return _size.Y;
            }
        }

        public bool Outlined
        {
            set { _Outlined = value; }
            get { return _Outlined; }
        }
        public bool Draggable
        {
            set { _Draggable = value; }
            get { return _Draggable; }
        }

        public Rectangle Rectangle
        {
            get
            {
                Vector2 parentoffset = new Vector2(0, 0);
                if (Parent != null)
                {
                    parentoffset = Parent.Position;
                }
                return new Rectangle((int)(Position.X + parentoffset.X), (int)(Position.Y + parentoffset.Y), _size.X, _size.Y);
            }
        }

        //public string Text { get; set; }
        #endregion

        #region Methods

        public MGPanel(Texture2D texture, SpriteFont font, int width, int height, Color backcolour)
        {
            _texture = texture;
            Width = width;
            Height = height;
            _font = font;
            Enabled = true;
            PenColour = Color.Black;
            BackColour = backcolour;
            canBeParent = false;
        }

        public MGPanel(Texture2D texture, SpriteFont font, Point size, Color backcolour)
        {
            _texture = texture;
            _size = size;
            _font = font;
            Enabled = true;
            PenColour = Color.Black;
            BackColour = backcolour;
            canBeParent = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 parentoffset = new Vector2(0, 0);
            if (Parent != null)
            {
                parentoffset = Parent.Position;
            }
            var colour = Color.White;

            //if (Enabled == false)
            //    colour = Color.Gray;
            //else if (_isHovering)
            //    colour = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, BackColour);

            if(_Outlined)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, Rectangle.Width, OutlineThickness), Color.Black);
                spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, OutlineThickness, Rectangle.Height), Color.Black);
                spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y+Rectangle.Height- OutlineThickness, Rectangle.Width, OutlineThickness), Color.Black);
                spriteBatch.Draw(_texture, new Rectangle((int)Position.X+Rectangle.Width- OutlineThickness, (int)Position.Y, OutlineThickness, Rectangle.Height), Color.Black);
            }

            //if (!string.IsNullOrEmpty(Text))
            //{
            //    var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2) + parentoffset.X;
            //    var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2) + parentoffset.Y;

            //    spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            //}
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            //_isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                //_isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed && Enabled)
                {
                    Click?.Invoke(this, new EventArgs());
                }

                if(Draggable && _currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Pressed && Enabled)
                {
                    Position = new Vector2(Position.X + (float)(_currentMouse.X - _previousMouse.X),
                        Position.Y + (float)(_currentMouse.Y - _previousMouse.Y));
                }
            }
        }
        #endregion
    }

    public class MGRadioButton : MGControl
    {
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }

    public class MGComboBox : MGControl
    {
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }

    public class MGGroupBox : MGControl
    {
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }

    public class MGNumericUpDown : MGControl
    {
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }

    public class MGLabel : MGControl
    {
        #region Fields
        //private MouseState _currentMouse;
        private SpriteFont _font;
        //private bool _isHovering;
        //private MouseState _previousMouse;
        private MGControl _parent;
        #endregion

        #region Properties
        //public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }



        public bool Enabled { get; set; }

        public MGControl Parent
        {
            get
            {
                if (_parent != null)
                    return _parent;
                else
                    return null;
            }
            set
            {
                if (value.canBeParent)
                {
                    _parent = value;
                }
            }
        }

        //public Rectangle Rectangle
        //{
        //    get
        //    {
        //        Vector2 parentoffset = new Vector2(0, 0);
        //        if (Parent != null)
        //        {
        //            parentoffset = Parent.Position;
        //        }
        //        return new Rectangle((int)(Position.X + parentoffset.X), (int)(Position.Y + parentoffset.Y), _texture.Width, _texture.Height);
        //    }
        //}

        public string Text { get; set; }
        #endregion

        #region Methods

        public MGLabel(SpriteFont font)
        {
            _font = font;
            Enabled = true;
            PenColour = Color.Black;
            canBeParent = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 parentoffset = new Vector2(0, 0);
            if (Parent != null)
            {
                parentoffset = Parent.Position;
            }
            var colour = Color.White;

            //if (Enabled == false)
            //    colour = Color.Gray;
            //else if (_isHovering)
            //    colour = Color.Gray;

            if (!string.IsNullOrEmpty(Text))
            {
                var x = parentoffset.X + Position.X;
                var y = parentoffset.Y + Position.Y;

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //_previousMouse = _currentMouse;
            //_currentMouse = Mouse.GetState();

            //var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            //_isHovering = false;

            //if (mouseRectangle.Intersects(Rectangle))
            //{
            //    _isHovering = true;

            //    if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed && Enabled)
            //    {
            //        Click?.Invoke(this, new EventArgs());
            //    }
            //}
        }

        #endregion
    }
}
