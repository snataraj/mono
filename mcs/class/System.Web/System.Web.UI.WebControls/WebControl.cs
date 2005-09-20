// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Peter Bartok	(pbartok@novell.com)
//
//

using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace System.Web.UI.WebControls {

	// CAS
	[AspNetHostingPermissionAttribute (SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermissionAttribute (SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	// attributes
#if NET_2_0
	[PersistChildrenAttribute (false, false)]
	[ParseChildrenAttribute (true, ChildControlType = typeof(Control))]
	[Themeable (true)]
#else	
	[PersistChildrenAttribute (false)]
	[ParseChildrenAttribute (true)]
#endif		
	public class WebControl : Control, IAttributeAccessor {
		Style style;
		HtmlTextWriterTag tag;
		string tag_name;
		AttributeCollection attributes;
		StateBag attribute_state;
		bool enabled;

		public WebControl (HtmlTextWriterTag tag) 
		{
			this.tag = tag;
			this.enabled = true;
		}

		protected WebControl () : this (HtmlTextWriterTag.Span) 
		{
		}

		protected WebControl (string tag) 
		{
			this.tag = HtmlTextWriterTag.Unknown;
			this.tag_name = tag;
			this.enabled = true;
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue("")]
		[WebSysDescription ("")]
		[WebCategory ("Behavior")]
		public virtual string AccessKey {
			get {
				return ViewState.GetString ("AccessKey", string.Empty);
			}
			set {
				if (value == null || value.Length < 2)
					ViewState ["AccessKey"] = value;
				else
					throw new ArgumentException ("AccessKey can only be null, empty or a single character", "value");
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription ("")]
		[WebCategory ("Behavior")]
		public AttributeCollection Attributes {
			get {
				if (attributes == null) {
					attribute_state = new StateBag ();
					if (IsTrackingViewState)
						attribute_state.TrackViewState ();
					
					attributes = new AttributeCollection (attribute_state);
				}
				return attributes;
			}
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue(typeof (Color), "")]
		[TypeConverter(typeof(System.Web.UI.WebControls.WebColorConverter))]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public virtual Color BackColor {
			get {
				if (style == null) 
					return Color.Empty;
				
				return style.BackColor;
			}
			set {
				ControlStyle.BackColor = value;
			}
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue(typeof (Color), "")]
		[TypeConverter(typeof(System.Web.UI.WebControls.WebColorConverter))]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public virtual Color BorderColor {
			get {
				if (style == null) 
					return Color.Empty;

				return style.BorderColor;
			}

			set {
				ControlStyle.BorderColor = value;
			}
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue(BorderStyle.NotSet)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public virtual BorderStyle BorderStyle {
			get {
				if (style == null) 
					return BorderStyle.NotSet;
				
				return style.BorderStyle;
			}
			set {
                                if (value < BorderStyle.NotSet || value > BorderStyle.Outset)
                                        throw new ArgumentOutOfRangeException ("value");

				ControlStyle.BorderStyle = value;
			}
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue(typeof (Unit), "")]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public virtual Unit BorderWidth {
			get {
				if (style == null) 
					return Unit.Empty;

				return style.BorderWidth;
			}
			set {
				ControlStyle.BorderWidth = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public Style ControlStyle {
			get {
				if (style == null) {
					style = this.CreateControlStyle ();

					if (IsTrackingViewState)
						style.TrackViewState ();
				}

				return style;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#if NET_2_0
		[EditorBrowsable (EditorBrowsableState.Never)]
#endif
		public bool ControlStyleCreated {
			get {
				return style != null;
			}
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue("")]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public virtual string CssClass {
			get {
				if (style == null) 
					return string.Empty;
				
				return style.CssClass;
			}
			set {
				ControlStyle.CssClass = value;
			}
		}

		[Bindable(true)]
		[DefaultValue(true)]
#if NET_2_0
		[Themeable (false)]
#endif		
		public virtual bool Enabled {
			get {
				return enabled;
			}

			set {
				if (enabled != value) {
					ViewState ["Enabled"] = value;
					enabled = value;
				}
			}
		}

#if NET_2_0
		[Browsable (true)]
		[MonoTODO]
		public virtual new bool EnableTheming
		{
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
#endif		

#if ONLY_1_1
		[DefaultValue(null)]
#endif		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public virtual FontInfo Font {
			get {
				// Oddly enough, it looks like we have to let it create the style
				// since we can't create a FontInfo without a style owner
				return ControlStyle.Font;
			}
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue(typeof (Color), "")]
		[TypeConverter(typeof(System.Web.UI.WebControls.WebColorConverter))]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public virtual Color ForeColor {
			get {
				if (style == null) 
					return Color.Empty;
				
				return style.ForeColor;
			}
			set {
				ControlStyle.ForeColor = value;
			}
		}

#if NET_2_0
		[Browsable (false)]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
		[MonoTODO]
		public bool HasAttributes 
		{
			get {
				throw new NotImplementedException ();
			}
		}
#endif		
		
#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue(typeof (Unit), "")]
		[WebSysDescription ("")]
		[WebCategory ("Layout")]
		public virtual Unit Height {
			get {
				if (style == null) 
					return Unit.Empty;
				
				return style.Height;
			}
			set {
				ControlStyle.Height = value;
			}
		}

#if NET_2_0
		[Browsable (true)]
		[MonoTODO]
		public virtual new string SkinID
		{
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
#endif		
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription ("")]
		[WebCategory ("Style")]
		public CssStyleCollection Style {
			get {
				return Attributes.CssStyle;
			}
		}

		[DefaultValue((short)0)]
		[WebSysDescription ("")]
		[WebCategory ("Behavior")]
		public virtual short TabIndex {
			get {
				return ViewState.GetShort ("TabIndex", 0);
			}
			set {
				ViewState ["TabIndex"] = value;
			}
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue("")]
#if NET_2_0
		[Localizable (true)]
#endif		
		[WebSysDescription ("")]
		[WebCategory ("Behavior")]
		public virtual string ToolTip {
			get {
				return ViewState.GetString ("ToolTip", string.Empty);
			}
			set {
				ViewState ["ToolTip"] = value;
			}
		}

#if ONLY_1_1
		[Bindable(true)]
#endif		
		[DefaultValue(typeof (Unit), "")]
		[WebSysDescription ("")]
		[WebCategory ("Layout")]
		public virtual Unit Width {
			get {
				if (style == null) 
					return Unit.Empty;
				
				return style.Width;
			}
			set {
				ControlStyle.Width = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual HtmlTextWriterTag TagKey {
			get {
				return tag;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected virtual string TagName {
			get {
				// do this here to avoid potentially costly lookups on every control
				if (tag_name == null)
					tag_name = HtmlTextWriter.StaticGetTagName (TagKey);
				
				return tag_name;
			}
		}

#if NET_2_0
		[MonoTODO]
		protected internal bool IsEnabled 
		{
			get {
				throw new NotImplementedException ();
			}
		}
#endif		
		

		public void ApplyStyle (Style s) 
		{
			if (s != null && !s.IsEmpty)
				ControlStyle.CopyFrom(s);
		}

		public void CopyBaseAttributes (WebControl controlSrc) 
		{
			object o;

			if (controlSrc == null) 
				return;

			o = controlSrc.ViewState ["Enabled"];
			if (o != null) {
				enabled = (bool)o;
			}

			o = controlSrc.ViewState ["AccessKey"];
			if (o != null)
				ViewState ["AccessKey"] = o;

			o = controlSrc.ViewState ["TabIndex"];
			if (o != null)
				ViewState ["TabIndex"] = o;

			o = controlSrc.ViewState ["ToolTip"];
			if (o != null)
				ViewState ["ToolTip"] = o;

			if (controlSrc.attributes != null)
				foreach (string s in controlSrc.attributes.Keys)
					Attributes [s] = controlSrc.attributes [s];
		}

		public void MergeStyle (Style s) 
		{
			if (s != null && !s.IsEmpty)
				ControlStyle.MergeWith(s);
		}

		public virtual void RenderBeginTag (HtmlTextWriter writer)
		{
			AddAttributesToRender (writer);
			
			if (TagKey == HtmlTextWriterTag.Unknown)
				writer.RenderBeginTag (TagName);
			else
				writer.RenderBeginTag (TagKey);
			
		}

		public virtual void RenderEndTag (HtmlTextWriter writer) 
		{
			writer.RenderEndTag ();
		}

		protected virtual void AddAttributesToRender (HtmlTextWriter writer) 
		{
			if (ID != null)
				writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);

			if (AccessKey != string.Empty)
				writer.AddAttribute (HtmlTextWriterAttribute.Accesskey, AccessKey);

			if (!enabled)
				writer.AddAttribute (HtmlTextWriterAttribute.Disabled, "disabled");

			if (ToolTip != string.Empty)
				writer.AddAttribute (HtmlTextWriterAttribute.Title, ToolTip);

			if (TabIndex != 0)
				writer.AddAttribute (HtmlTextWriterAttribute.Tabindex, TabIndex.ToString ());

			if (style != null && !style.IsEmpty)
				style.AddAttributesToRender(writer, this);

			if (attributes != null)
				foreach(string s in attributes.Keys)
					writer.AddAttribute (s, attributes [s]);
		}

		protected virtual Style CreateControlStyle() 
		{
			style = new Style (ViewState);
			return style;
		}

		protected override void LoadViewState (object savedState) 
		{
			if (savedState == null) {
				base.LoadViewState(null);
				return;
			}

			Triplet triplet = (Triplet) savedState;

			base.LoadViewState (triplet.First);
			
			if (triplet.Second != null) {
				if (attribute_state == null) {
					attribute_state = new StateBag ();
					if (IsTrackingViewState) 
						attribute_state.TrackViewState ();
				}
				attribute_state.LoadViewState (triplet.Second);
				attributes = new AttributeCollection(attribute_state);
			}

			if (triplet.Third != null) {
				if (style == null)
					style = CreateControlStyle ();

				style.LoadViewState (triplet.Third);
			}

			enabled = ViewState.GetBool("Enabled", true);
		}

#if NET_2_0
		protected internal
#else		
		protected
#endif		
		override void Render (HtmlTextWriter writer)
		{
			RenderBeginTag (writer);
			RenderContents (writer);
			RenderEndTag (writer);
		}

#if NET_2_0
		protected internal
#else		
		protected
#endif		
		virtual void RenderContents (HtmlTextWriter writer)
		{
			base.Render (writer);
		}

		protected override object SaveViewState () 
		{
			object view_state;
			object attr_view_state = null;
			object style_view_state = null;

			view_state = base.SaveViewState ();

			if (attribute_state != null)
				attr_view_state = attribute_state.SaveViewState ();
		
			if (style != null)
				style_view_state = style.SaveViewState ();
			
			if (view_state == null && attr_view_state == null && style_view_state == null)
				return null;
			
			return new Triplet (view_state, attr_view_state, style_view_state);
		}

		protected override void TrackViewState() 
		{
			if (style != null)
				style.TrackViewState ();

			if (attribute_state != null)
				attribute_state.TrackViewState ();

			base.TrackViewState ();
		}

		string IAttributeAccessor.GetAttribute (string key) 
		{
			if (attributes != null)
				return attributes [key];

			return null;
		}

		void IAttributeAccessor.SetAttribute (string key, string value) 
		{
			Attributes [key] = value;
		}
	}
}
