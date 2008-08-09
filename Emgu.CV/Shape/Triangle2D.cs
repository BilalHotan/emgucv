using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Emgu.CV
{
    /// <summary>
    /// A 2D triangle
    /// </summary>
    /// <typeparam name="T">The depth of the triangle</typeparam>
    public class Triangle2D<T> : IConvexPolygon<T> where T: IComparable, new()
    {
        /// <summary>
        /// The vertices for this triangle
        /// </summary>
        private Point2D<T>[] _vertices;

        /// <summary>
        /// Get or set the vertices of this triangle
        /// </summary>
        public Point2D<T>[] Vertices
        {
            get { return _vertices; }
            set 
            {
                Debug.Assert(value.Length == 3, "The number of vertices for triangle must be 3");
                _vertices = value; 
            }
        }
	
        /// <summary>
        /// Create a triangle using the specific vertices
        /// </summary>
        /// <param name="v1">The first vertex</param>
        /// <param name="v2">The second vertex</param>
        /// <param name="v3">The third vertex</param>
        public Triangle2D(Point2D<T> v1, Point2D<T> v2, Point2D<T> v3 )
        {
            _vertices = new Point2D<T>[] { v1, v2, v3 };
        }


        /// <summary>
        /// Get the area of this triangle
        /// </summary>
        public double Area
        {
            get
            {
                Point2D<double>[] vertices = Array.ConvertAll<Point2D<T>, Point2D<double>>(Vertices, delegate(Point2D<T> p) { return p.Convert<double>(); });
                LineSegment2D<double>[] edges =  PointCollection.PolyLine<double>(vertices, true);
                double[] edgeLengths = Array.ConvertAll<LineSegment2D<double>, double>(edges, delegate(LineSegment2D<double> line) { return line.Length; });

                #region use Heron's formula to find the area of the triangle
                double a = edgeLengths[0];
                double b = edgeLengths[1];
                double c = edgeLengths[2];
                double s = (a + b + c) / 2.0;
                return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
                #endregion
            }
        }

        /// <summary>
        /// Compare two triangles and return true if equal
        /// </summary>
        /// <param name="tri">the other triangles to compare with</param>
        /// <returns>true if the two triangles equals, false otherwise</returns>
        public bool Equals(Triangle2D<T> tri)
        {
            Point2D<T>[] v2 = tri.Vertices;
            Point2D<T>[] v1 = Vertices;

            bool res = true;
            foreach (Point2D<T> v in v1)
                res = res && Array.Exists<Point2D<T>>(v2, v.Equals) ;

            return res;
        }
    }
}
