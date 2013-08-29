using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System;

namespace Texture_Grabber
{
    /// <summary>
    /// A PictureBox control extended to allow a variety of interpolations.
    /// </summary>
    class InterpolatedBox : PictureBox
    {
        #region Interpolation Property
        /// <summary>Backing Field</summary>
        private InterpolationMode interpolation = InterpolationMode.Default;

        /// <summary>
        /// The interpolation used to render the image.
        /// </summary>
        [DefaultValue(typeof(InterpolationMode), "Default"),
        Description("The interpolation used to render the image.")]
        public InterpolationMode Interpolation
        {
            get { return interpolation; }
            set
            {
                if (value == InterpolationMode.Invalid)
                    throw new ArgumentException("\"Invalid\" is not a valid value."); // (Duh!)

                interpolation = value;
                Invalidate(); // Image should be redrawn when a different interpolation is selected
            }
        }
        #endregion

        /// <summary>
        /// Overridden to modify rendering behavior.
        /// </summary>
        /// <param name="pe">Painting event args.</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Before the PictureBox renders the image, we modify the
            // graphics object to change the interpolation.

            // Set the selected interpolation.
            pe.Graphics.InterpolationMode = interpolation;
            // Certain interpolation modes (such as nearest neighbor) need
            // to be offset by half a pixel to render correctly.
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            // Allow the PictureBox to draw.
            base.OnPaint(pe);
        }
    }
}
