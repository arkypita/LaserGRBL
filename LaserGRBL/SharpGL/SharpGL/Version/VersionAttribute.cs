using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.Version
{
    /// <summary>
    /// Allows a version to be specified as metadata on a field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class VersionAttribute : Attribute
    {
        private readonly int major;
        private readonly int minor;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionAttribute"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        public VersionAttribute(int major, int minor)
        {
            this.major = major;
            this.minor = minor;
        }

        /// <summary>
        /// Determines whether this version is at least as high as the version specified in the parameters.
        /// </summary>
        /// <param name="major">The major version.</param>
        /// <param name="minor">The minor version.</param>
        /// <returns>True if this version object is at least as high as the version specified by <paramref name="major"/> and <paramref name="minor"/>.</returns>
        public bool IsAtLeastVersion(int major, int minor)
        {
            //  If major versions match, we care about minor. Otherwise, we only care about major.
            if (this.major == major)
                return this.major >= major && this.minor >= minor;
            return this.major >= major;
        }

        /// <summary>
        /// Gets the version attribute of an enumeration value <paramref name="enumeration"/>.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        /// <returns>The <see cref="VersionAttribute"/> defined on <paramref name="enumeration "/>, or null of none exists.</returns>
        public static VersionAttribute GetVersionAttribute(Enum enumeration)
        {
            //  Get the attribute from the enumeration value (if it exists).
            return enumeration
                    .GetType()
                    .GetMember(enumeration.ToString())
                    .Single()
                    .GetCustomAttributes(typeof(VersionAttribute), false)
                    .OfType<VersionAttribute>()
                    .FirstOrDefault();
        }

        /// <summary>
        /// Gets the major version number.
        /// </summary>
        public int Major
        {
            get { return major; }
        }

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        public int Minor
        {
            get { return minor; }
        }
    }
}
