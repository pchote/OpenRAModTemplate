#region Copyright & License Information
/*
 * Copyright 2007-2020 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Graphics;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Primitives;
using OpenRA.Widgets;

namespace OpenRA.Mods.Example.Widgets
{
	public class OverlayColorPreviewManagerWidget : ColorPreviewManagerWidget
	{
		Color cachedColor;
		WorldRenderer worldRenderer;
		IPalette preview;

		[ObjectCreator.UseCtor]
		public OverlayColorPreviewManagerWidget(WorldRenderer worldRenderer)
			: base(worldRenderer)
		{
			this.worldRenderer = worldRenderer;
		}

		public override void Initialize(WidgetArgs args)
		{
			base.Initialize(args);
			preview = worldRenderer.Palette(PaletteName).Palette;
		}

		public override void Tick()
		{
			if (cachedColor == Color)
				return;
			cachedColor = Color;

			var newPalette = new MutablePalette(preview);

			foreach (var i in RemapIndices)
			{
				var bw = (float)(((preview[i] & 0xff) + ((preview[i] >> 8) & 0xff) + ((preview[i] >> 16) & 0xff)) / 3) / 0xff;
				var dstR = bw > .5 ? 1 - (1 - 2 * (bw - .5)) * (1 - (float)Color.R / 0xff) : 2 * bw * ((float)Color.R / 0xff);
				var dstG = bw > .5 ? 1 - (1 - 2 * (bw - .5)) * (1 - (float)Color.G / 0xff) : 2 * bw * ((float)Color.G / 0xff);
				var dstB = bw > .5 ? 1 - (1 - 2 * (bw - .5)) * (1 - (float)Color.B / 0xff) : 2 * bw * ((float)Color.B / 0xff);
				newPalette[i] = (preview[i] & 0xff000000) | ((uint)(dstR * 0xff) << 16) | ((uint)(dstG * 0xff) << 8) | (uint)(dstB * 0xff);
			}

			worldRenderer.ReplacePalette(PaletteName, newPalette);
		}
	}
}
