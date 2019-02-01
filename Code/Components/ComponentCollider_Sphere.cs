using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    public class ComponentCollider_Sphere : ComponentCollider
    {
        public float radius;

        public ComponentCollider_Sphere(float radius, ComponentTransform transform) : base(transform)
        {
            this.radius = radius;
        }

        public ComponentCollider_Sphere(float radius, bool isTriggerCollider, ComponentTransform transform) : base(transform, isTriggerCollider)
        {
            this.radius = radius;
        }

        public override bool CheckCollision(ComponentCollider other, out float depenetration, out Vector3 normal)
        {
            depenetration = 0;
            normal = Vector3.Zero;
            if(other is ComponentCollider_Box)
            {
                Bounds boxBounds = (other as ComponentCollider_Box).collisionBounds;
                Vector3 boundsDelta = new Vector3(other.transform.position.X, 0, other.transform.position.Z); 
                Vector3 point = new Vector3(transform.position.X, 0, transform.position.Z);
                float distToLine;

                // ==== X line ====
                Vector3 line_X1 = boundsDelta;
                line_X1.X += boxBounds.min.X;
                line_X1.Z += boxBounds.center.Z;
                Vector3 line_X2 = boundsDelta;
                line_X2.X += boxBounds.max.X;
                line_X2.Z += boxBounds.center.Z;
                if (Utilities.Distance(line_X1, point) > Utilities.Distance(line_X2, point)) Utilities.Swap<Vector3>(ref line_X1, ref line_X2);
                
                // ==== Z line =====
                Vector3 line_Z1 = boundsDelta;
                line_Z1.X += boxBounds.center.X;
                line_Z1.Z += boxBounds.min.Z;
                Vector3 line_Z2 = boundsDelta;
                line_Z2.X += boxBounds.center.X;
                line_Z2.Z += boxBounds.max.Z;
                if (Utilities.Distance(line_Z1, point) > Utilities.Distance(line_Z2, point)) Utilities.Swap<Vector3>(ref line_Z1, ref line_Z2);

                Vector3 pX, pZ;

                if(Utilities.ProjectPoint(point, line_X1, line_X2, out pX))
                {
                    distToLine = pX.Length - boxBounds.extends.Z;
                    if (distToLine - radius < 0)
                    {
                        depenetration = (radius - distToLine) + float.Epsilon;
                        normal = pX.Normalized();
                        return true;
                    }
                }
                else if(Utilities.ProjectPoint(point, line_Z1, line_Z2, out pZ))
                {
                    distToLine = pZ.Length - boxBounds.extends.X;
                    if (distToLine - radius < 0)
                    {
                        depenetration = (radius - distToLine) + float.Epsilon;
                        normal = pZ.Normalized();
                        return true;
                    }
                }
                else
                {
                    Vector3 closestCorner = Vector3.Zero;
                    float closestDistance = float.PositiveInfinity;
                    for(int i = 0; i < 4; i++)
                    {
                        Vector3 corner = boxBounds.corners[i] + boundsDelta;
                        float distance = Utilities.Distance(corner, point);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestCorner = corner;
                        }
                    }

                    distToLine = Utilities.Distance(closestCorner, point);
                    if (distToLine - radius < 0)
                    {
                        depenetration = (radius - distToLine) + float.Epsilon;
                        normal = (point - closestCorner).Normalized();
                        return true;
                    }
                }
            }

            if(other is ComponentCollider_Sphere)
            {
                float distance = Utilities.Distance(this.transform.position, other.transform.position);
                float radius1 = this.radius * 2;
                float radius2 = (other as ComponentCollider_Sphere).radius * 2;
                if (distance < radius1 + radius2) 
                {
                    depenetration = (radius2 + radius1) - distance;
                    normal = (this.transform.position - other.transform.position).Normalized();
                    return true;
                }
            }


            return false;
        }
    }
}
