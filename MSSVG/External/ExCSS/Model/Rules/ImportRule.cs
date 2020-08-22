using ExCSS.Model;
using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class ImportRule : RuleSet, ISupportsMedia
    {
        private string _href;
        private readonly MediaTypeList _media;

        public ImportRule() 
        {
            _media = new MediaTypeList();
            RuleType = RuleType.Import;
        }
      
        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }

        public MediaTypeList Media
        {
            get { return _media; }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return _media.Count > 0
                ? string.Format("@import url({0}) {1};", _href, _media).NewLineIndent(friendlyFormat, indentation)
                : string.Format("@import url({0});", _href).NewLineIndent(friendlyFormat, indentation);
        }
    }
}
