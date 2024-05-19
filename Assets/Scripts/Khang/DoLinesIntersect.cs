using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace KhangLibrary
{
    public class LineIntersection3D : MonoBehaviour
    {
        /// <summary>
        /// Định nghĩa đường thẳng thông qua hai điểm
        /// </summary>
        public struct Line
        {
            public Vector3 point1;
            public Vector3 point2;
        }


        /// <summary>
        /// Hàm kiểm tra sự chạm giữa hai đường thẳng
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public bool DoLinesIntersect(Line line1, Line line2)
        {
            float crossProduct1 = CrossProduct(line1.point1, line1.point2, line2.point1);
            float crossProduct2 = CrossProduct(line1.point1, line1.point2, line2.point2);

            float crossProduct3 = CrossProduct(line2.point1, line2.point2, line1.point1);
            float crossProduct4 = CrossProduct(line2.point1, line2.point2, line1.point2);

            return (crossProduct1 * crossProduct2 < 0) && (crossProduct3 * crossProduct4 < 0);
        }

        /// <summary>
        /// cho vào 1 đoạn thẳng và 1 điểm, trả về điểm trên đoạn thẳng vuông gốc với điểm 
        /// </summary>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 GetPerpendicularPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector2 lineStartXZ = new Vector2(lineStart.x, lineStart.z);
            Vector2 lineEndXZ = new Vector2(lineEnd.x, lineEnd.z);
            Vector2 pointXZ = new Vector2(point.x, point.z);

            Vector2 lineDirectionXZ = (lineEndXZ - lineStartXZ).normalized;
            Vector2 fromLineStartToPointXZ = pointXZ - lineStartXZ;
            float distanceXZ = Vector2.Dot(fromLineStartToPointXZ, lineDirectionXZ);

            // Chuyển kết quả trở lại thành vector 3D
            return new Vector3(lineStart.x + distanceXZ * lineDirectionXZ.x, point.y, lineStart.z + distanceXZ * lineDirectionXZ.y);
        }
        /// <summary>
        /// kiểm tra 2 đoạn thẳng có vuông góc không 
        /// </summary>
        /// <param name="startPoint1"></param>
        /// <param name="endPoint1"></param>
        /// <param name="startPoint2"></param>
        /// <param name="endPoint2"></param>
        /// <returns></returns>
        public static bool ArePerpendicular(Vector3 startPoint1, Vector3 endPoint1, Vector3 startPoint2, Vector3 endPoint2)
        {
            // Tính vectơ chỉ phương của 2 đường thẳng trên trục x và z
            Vector3 direction1 = new Vector3(endPoint1.x - startPoint1.x, 0f, endPoint1.z - startPoint1.z);
            Vector3 direction2 = new Vector3(endPoint2.x - startPoint2.x, 0f, endPoint2.z - startPoint2.z);

            // Kiểm tra xem tích vô hướng của hai vectơ chỉ phương có bằng 0 hay không
            return Mathf.Approximately(Vector3.Dot(direction1, direction2), 0f);
        }
        /// <summary>
        /// Song song
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <returns></returns>
        public static bool AreParallelFromPoints(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            // Tính vector chỉ phương của 2 đường thẳng
            Vector3 d1 = (p2 - p1).normalized;
            Vector3 d2 = (p4 - p3).normalized;

            /*// Tính hệ số góc của 2 đường thẳng
            float a1 = d1.y / d1.x;
            float a2 = d2.y / d2.x;*/

            /*// So sánh hệ số góc của 2 đường thẳng
            return a1 == a2;*/

            // Kiểm tra xem hai hướng có giống nhau hay không (song song)
            return Vector3.Dot(d1, d2) == 1f || Vector3.Dot(d1, d2) == -1f;
        }

        /// <summary>
        /// Hàm tính hệ số góc dựa trên hai điểm
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        private static float GetSlopeXZ(Vector3 point1, Vector3 point2)
        {
            if (point2.x - point1.x == 0)
            {
                return float.PositiveInfinity;
            }

            return (point2.z - point1.z) / (point2.x - point1.x);
        }

        /// <summary>
        /// Hàm tính cross product giữa hai vector (x, z)
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <returns></returns>
        float CrossProduct(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            return (point2.x - point1.x) * (point3.z - point1.z) - (point2.z - point1.z) * (point3.x - point1.x);
        }

        public static void MoveToDirection(GameObject obj, Vector3 direction, float value)
        {

        }
    }
}
