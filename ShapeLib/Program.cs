using System;
using ShapeLib.Bridge1;
using ShapeLib.Bridge2;
using Rectangle = ShapeLib.Bridge1.Rectangle;
using RectangleDrawer = ShapeLib.Bridge1.RectangleDrawer;

namespace ShapeLib
{
    public enum Color
    {
        Red, Green, Blue
    }

    //Мост, с абстракцией
    namespace Bridge1
    {
        public interface IDrawer<T> where T : Shape<T>
        {
            void Draw(Shape<T> shape);
        }

        public abstract class Shape<T> where T : Shape<T>
        {
            private readonly IDrawer<T> _drawer;

            protected Shape(IDrawer<T> drawer)
            {
                _drawer = drawer;
            }

            public virtual void Draw()
            {
                _drawer.Draw(this);
            }
        }

        public class Rectangle : Shape<Rectangle>
        {
            public Rectangle(IDrawer<Rectangle> drawer) : base(drawer)
            {
            }

            public override string ToString()
            {
                return "Rectangle";
            }
        }

        public class RectangleDrawer : IDrawer<Rectangle>
        {
            public void Draw(Shape<Rectangle> shape)
            {
                Console.WriteLine(shape.ToString());
            }
        }

        public class Circle : Shape<Circle>
        {
            public Circle(IDrawer<Circle> drawer) : base(drawer)
            {
            }

            public override string ToString()
            {
                return "Circle";
            }
        }

        public class CircleDrawer : IDrawer<Circle>
        {
            public void Draw(Shape<Circle> shape)
            {
                Console.WriteLine(shape.ToString());
            }
        }

        public class RedRectangle : Rectangle
        {
            public RedRectangle(IDrawer<Rectangle> drawer) : base(drawer)
            {
            }

            public override string ToString()
            {
                return $"Red {base.ToString()}";
            }
        }

    }

    //Мост с интерфейсом
    namespace Bridge2
    {
        public interface IShape
        {
            void Draw();
        }

        public interface IShapeDrawer<T> where T: IShape
        {
            void Draw(T shape);
        }

        public class Rectangle: IShape
        {
            private readonly IShapeDrawer<Rectangle> _drawer;

            public Rectangle(IShapeDrawer<Rectangle> drawer)
            {
                _drawer = drawer;
            }

            public double Width { get; set; }
            public double Height { get; set; }

            public void Draw()
            {
                _drawer.Draw(this);
            }

            public override string ToString()
            {
                return nameof(Rectangle);
            }
        }

        public class RectangleDrawer : IShapeDrawer<Rectangle>
        {
            public void Draw(Rectangle shape)
            {
                Console.WriteLine($"{shape.ToString()}: {shape.Width}x{shape.Height}");
            }
        }

        public class ColoredRectangleDrawer : IShapeDrawer<Rectangle>
        {
            private readonly Color _color;

            public ColoredRectangleDrawer(Color color)
            {
                _color = color;
            }

            public void Draw(Rectangle shape)
            {
                Console.WriteLine($"{_color.ToString()} : {shape.ToString()}: {shape.Width}x{shape.Height}");
            }
        }
    }

    //Что-то похожее на команду
    namespace Command
    {
        public abstract class Shape
        {
            //что-то общее
        }

        public class Rectangle : Shape
        {
            public int Height { get; set; }
            public int Width { get; set; }

            public override string ToString()
            {
                return $"{nameof(Bridge1.Rectangle)} - {Height}x{Width}";
            }
        }

        public interface IShapeDrawer<T> where T: Shape
        {
            void Draw(T shape);
        }

        public class ColoredRectangle : Rectangle
        {
            public Color Color { get; set; }

            public override string ToString()
            {
                return $"{Color.ToString()} - {base.ToString()}";
            }
        }

        public class RectangleDrawer : IShapeDrawer<Rectangle>
        {
            public void Draw(Rectangle shape)
            {
                Console.WriteLine(shape.ToString());
            }
        }

        public class ColoredRectangleDrawer : IShapeDrawer<ColoredRectangle>
        {
            public void Draw(ColoredRectangle shape)
            {
                Console.WriteLine(shape.ToString());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            //bridge 1
            var shape = new Rectangle(new RectangleDrawer());
            shape.Draw();

            var shape2 = new RedRectangle(new RectangleDrawer());
            shape2.Draw();

            //bridge2
            var shape3 = new Bridge2.Rectangle(new ColoredRectangleDrawer(Color.Red));
            shape3.Draw();

            //что-то похожее на команду
            var shape4 = new Command.ColoredRectangle(){Color = Color.Blue, Width = 10, Height = 7};
            (new Command.ColoredRectangleDrawer()).Draw(shape4);

            Console.WriteLine("Hello World!");
        }
    }
}
