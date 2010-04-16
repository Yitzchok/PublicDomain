using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// http://www.javascriptkit.com/dhtmltutors/csscursors.shtml
    /// </summary>
    public static class Cursors
    {
        /// <summary>
        /// cursor: pointer; cursor: hand;
        /// </summary>
        public const string HandAndPointer = "cursor: pointer; cursor: hand;";

        /// <summary>
        /// Gets the cursor style. Returns 'cursor: {0};' without the quotes,
        /// and with {0} replaced by the cursor style. This method may
        /// also return a compound style, such as 'cursor: {0};cursor: {1};'.
        /// Always ends in a trailing semicolon;
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <returns></returns>
        public static string GetStyle(CursorStyle cursor)
        {
            string result = null;
            switch (cursor)
            {
                case CursorStyle.ResizeEast:
                    result = "cursor: e-resize;";
                    break;
                case CursorStyle.ResizeNorth:
                    result = "cursor: n-resize;";
                    break;
                case CursorStyle.ResizeNorthEast:
                    result = "cursor: ne-resize;";
                    break;
                case CursorStyle.ResizeNorthWest:
                    result = "cursor: nw-resize;";
                    break;
                case CursorStyle.ResizeSouth:
                    result = "cursor: s-resize;";
                    break;
                case CursorStyle.ResizeSouthEast:
                    result = "cursor: se-resize;";
                    break;
                case CursorStyle.ResizeSouthWest:
                    result = "cursor: sw-resize;";
                    break;
                case CursorStyle.ResizeWest:
                    result = "cursor: w-resize;";
                    break;
                case CursorStyle.AllScroll:
                    result = "cursor: all-scroll;";
                    break;
                case CursorStyle.ColumnResize:
                    result = "cursor: col-resize;";
                    break;
                case CursorStyle.RowResize:
                    result = "cursor: row-resize;";
                    break;
                case CursorStyle.NoDrop:
                    result = "cursor: no-drop;";
                    break;
                case CursorStyle.NotAllowed:
                    result = "cursor: not-allowed;";
                    break;
                case CursorStyle.VerticalText:
                    result = "cursor: vertical-text;";
                    break;
                case CursorStyle.Inherit:
                    return "";
                case CursorStyle.HandAndPointer:
                    result = HandAndPointer;
                    break;
                default:
                    result = "cursor: " + cursor.ToString().ToLower() + ";";
                    break;
            }
            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CursorStyle
    {
        /// <summary>
        /// Support: All Browsers
        /// </summary>
        Auto,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        Default,

        /// <summary>
        /// Support: Only IE. Use HandAndPointer instead
        /// </summary>
        Hand,

        /// <summary>
        /// Support: NS6+/IE6+ only. Use HandAndPointer instead
        /// </summary>
        Pointer,

        /// <summary>
        /// Support: Cross browser
        /// </summary>
        HandAndPointer,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        Crosshair,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        Text,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        Wait,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        Help,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        Inherit,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        Move,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        ResizeEast,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        ResizeNorthEast,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        ResizeNorthWest,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        ResizeNorth,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        ResizeSouthEast,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        ResizeSouthWest,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        ResizeSouth,

        /// <summary>
        /// Support: All Browsers
        /// </summary>
        ResizeWest,

        /// <summary>
        /// Support: IE6+
        /// </summary>
        Progress,

        /// <summary>
        /// Support: IE6+
        /// </summary>
        AllScroll,

        /// <summary>
        /// Support: IE6+
        /// </summary>
        ColumnResize,

        /// <summary>
        /// Support: IE6+
        /// </summary>
        NoDrop,

        /// <summary>
        /// Support: IE6+
        /// </summary>
        NotAllowed,

        /// <summary>
        /// Support: IE6+
        /// </summary>
        RowResize,

        /// <summary>
        /// Support: IE6+
        /// </summary>
        VerticalText
    }
}
