using System.Windows.Input;

namespace Extensions.Wpf.Gestures
{
    public class MouseWheelGesture : MouseGesture
    {
        public MouseWheelGesture()
            : base(MouseAction.WheelClick)
        {
        }

        public MouseWheelGesture(ModifierKeys modifiers)
            : base(MouseAction.WheelClick, modifiers)
        {
        }

        public MouseWheelDirection Direction { get; set; }

        /// <summary>
        ///     Represents a mouse wheel down gesture with control and shift key held.
        /// </summary>
        public static MouseWheelGesture CtrlShiftWheelDown
        {
            get
            {
                return new MouseWheelGesture(ModifierKeys.Control | ModifierKeys.Shift) {Direction = MouseWheelDirection.Down};
            }
        }

        /// <summary>
        ///     Represents a mouse wheel up gesture with control and shift key held.
        /// </summary>
        public static MouseWheelGesture CtrlShiftWheelUp
        {
            get
            {
                return new MouseWheelGesture(ModifierKeys.Control | ModifierKeys.Shift) {Direction = MouseWheelDirection.Up};
            }
        }

        /// <summary>
        ///     Represents a mouse wheel down gesture with control key held.
        /// </summary>
        public static MouseWheelGesture CtrlWheelDown
        {
            get { return new MouseWheelGesture(ModifierKeys.Control) {Direction = MouseWheelDirection.Down}; }
        }

        /// <summary>
        ///     Represents a mouse wheel up gesture with control key held.
        /// </summary>
        public static MouseWheelGesture CtrlWheelUp
        {
            get { return new MouseWheelGesture(ModifierKeys.Control) {Direction = MouseWheelDirection.Up}; }
        }

        /// <summary>
        ///     Represents a mouse wheel down gesture with shift key held.
        /// </summary>
        public static MouseWheelGesture ShiftWheelDown
        {
            get { return new MouseWheelGesture(ModifierKeys.Shift) {Direction = MouseWheelDirection.Down}; }
        }

        /// <summary>
        ///     Represents a mouse wheel up gesture with shift key held.
        /// </summary>
        public static MouseWheelGesture ShiftWheelUp
        {
            get { return new MouseWheelGesture(ModifierKeys.Shift) {Direction = MouseWheelDirection.Up}; }
        }

        /// <summary>
        ///     Represents a mouse wheel down gesture.
        /// </summary>
        public static MouseWheelGesture WheelDown
        {
            get { return new MouseWheelGesture(ModifierKeys.None) {Direction = MouseWheelDirection.Down}; }
        }

        /// <summary>
        ///     Represents a mouse wheel up gesture.
        /// </summary>
        public static MouseWheelGesture WheelUp
        {
            get { return new MouseWheelGesture(ModifierKeys.None) {Direction = MouseWheelDirection.Up}; }
        }

        /// <summary>
        ///     Determines whether <see cref="T:System.Windows.Input.MouseGesture" /> matches the input associated with the
        ///     specified <see cref="T:System.Windows.Input.InputEventArgs" /> object.
        /// </summary>
        /// <returns>
        ///     true if the event data matches this <see cref="T:System.Windows.Input.MouseGesture" />; otherwise, false.
        /// </returns>
        /// <param name="targetElement">The target.</param>
        /// <param name="inputEventArgs">The input event data to compare with this gesture.</param>
        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            if (!base.Matches(targetElement, inputEventArgs)) return false;
            if (!(inputEventArgs is MouseWheelEventArgs)) return false;
            var args = (MouseWheelEventArgs) inputEventArgs;
            switch (Direction)
            {
                case MouseWheelDirection.None:
                    return args.Delta == 0;
                case MouseWheelDirection.Up:
                    return args.Delta > 0;
                case MouseWheelDirection.Down:
                    return args.Delta < 0;
                default:
                    return false;
            }
        }
    }
}