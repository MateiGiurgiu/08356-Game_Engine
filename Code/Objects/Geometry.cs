using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ModelLoader;

namespace OpenGL_Game.Objects
{
    public class Bounds
    {
        public Vector3 min;
        public Vector3 max;
        public Vector3 center
        {
            get
            {
                return (min + max) / 2f;
            }
        }
        public Vector3 size
        {
            get
            {
                return max - min;
            }
        }
        public Vector3 extends
        {
            get
            {
                return size / 2f;
            }
        }

        public Vector3[] corners
        {
            get;
            private set;
        }
        public void ComputeCorners()
        {
            corners = new Vector3[4];
            corners[0] = new Vector3(min.X, 0, min.Z);
            corners[1] = new Vector3(min.X, 0, max.Z);
            corners[2] = new Vector3(max.X, 0, min.Z);
            corners[3] = new Vector3(max.X, 0, max.Z);
        }
    }

    public class Geometry
    {
        public struct Vertex
        {
            public Vector3 position;
            public Vector3 normal;
            public Vector2 uv;

            public Vertex(Vector3 position, Vector3 normal, Vector2 uv)
            {
                this.position = position;
                this.normal = normal;
                this.uv = uv;
            }

            public float this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return position.X;
                        case 1:
                            return position.Y;
                        case 2:
                            return position.Z;
                        case 3:
                            return normal.X;
                        case 4:
                            return normal.Y;
                        case 5:
                            return normal.Z;
                        case 6:
                            return uv.X;
                        case 7:
                            return uv.Y;
                        default:
                            return 0;
                    }
                }
            }

            public static bool operator ==(Vertex lhs, Vertex rhs)
            {
                bool p = lhs.position == rhs.position;
                bool n = lhs.normal == rhs.normal;
                bool u = lhs.uv == rhs.uv;
                return p & n & u;
            }

            public static bool operator !=(Vertex lhs, Vertex rhs)
            {
                bool p = lhs.position == rhs.position;
                bool n = lhs.normal == rhs.normal;
                bool u = lhs.uv == rhs.uv;
                return !(p & n & u);
            }
        }

        // mesh data
        public readonly List<Vertex> vertices = new List<Vertex>();
        public readonly List<int> indices = new List<int>();
        public readonly Bounds bounds = new Bounds();

        // OpenGL related
        private int VAO = -1;
        private int VBO_vertices = -1;
        private int VBO_elements = -1;
        int triangles = 0;

        public Geometry(string filename)
        {
            if (File.Exists(filename))
            {
                string extension = filename.Substring(filename.IndexOf('.'));
                if (extension == ".obj")
                {
                    LoadMeshDataFrom_OBJ(filename);
                }
                else
                {
                    throw new Exception("Unsupported extension. Please use .obj files");
                }

                SetUpBuffers();
                bounds = new Bounds();
                SetUpBounds();
            }
            else
            {
                throw new FileNotFoundException("Unable to open \"" + filename + "\", does not exist.");
            }
        }

        private void LoadMeshDataFrom_OBJ(string filename)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(filename))
                {
                    List<Vector3> positions = new List<Vector3>();
                    List<Vector2> uvs = new List<Vector2>();
                    List<Vector3> normals = new List<Vector3>();

                    for (string line = streamReader.ReadLine(); line != null; line = streamReader.ReadLine())
                    {
                        if (line == string.Empty) continue;

                        List<string> data = line.ToLower().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        string type = data[0];

                        switch (type)
                        {
                            case "v":
                                positions.Add(new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3])));
                                break;
                            case "vt":
                                uvs.Add(new Vector2(float.Parse(data[1]), 1 - float.Parse(data[2])));
                                break;
                            case "vn":
                                normals.Add(new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3])));
                                break;
                            case "f":
                                /* 
                                 * struct of face data "f"
                                 * f 5/1/1 1/2/1 4/3/1
                                 *    v1    v2    v3
                                 * v1 will use for position index 5, for uv index 1 and for normals index 1
                                 * v2 will use for position index 1, for uv index 2 and for normals index 1
                                 * v2 will use for position index 4, for uv index 3 and for normals index 1
                                */
                                for (int i = 1; i <= 3; i++)
                                {
                                    string[] attributeIndices = data[i].Split('/');
                                    int indexPosition = int.Parse(attributeIndices[0]) - 1;
                                    int indexUV = int.Parse(attributeIndices[1]) - 1;
                                    int indexNormal = int.Parse(attributeIndices[2]) - 1;

                                    Vertex vertexToAdd = new Vertex(positions[indexPosition], normals[indexNormal], uvs[indexUV]);

                                    // go through all the vertices in the list and check if there is already a similar one in the list
                                    // if it is, use that one, if not add a new one and use the index of the newly added one
                                    // this prevents vertex duplication
                                    int index = -1;
                                    for (int j = 0; j < vertices.Count; j++)
                                    {
                                        if (vertices[j] == vertexToAdd)
                                        {
                                            index = j;
                                            break;
                                        }
                                    }
                                    // if we could't find any similat vertex, let's add this one to the end of the list
                                    if (index == -1)
                                    {
                                        vertices.Add(vertexToAdd);
                                        index = vertices.Count - 1;
                                    }

                                    // let's also add this index to the indices list
                                    indices.Add(index);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private float[] GetDataFromVertexList()
        {
            List<float> data = new List<float>();
            foreach (Vertex v in vertices)
            {
                for (int i = 0; i < 8; i++)
                {
                    data.Add(v[i]);
                }
            }
            return data.ToArray<float>();
        }

        private void SetUpBuffers()
        {
            if (vertices.Count == 0) return;

            // generate Vertex Array Object
            GL.GenVertexArrays(1, out VAO);
            GL.BindVertexArray(VAO);

            // Generate VBOs
            GL.GenBuffers(1, out VBO_vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_vertices);
            GL.GenBuffers(1, out VBO_elements);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, VBO_elements);

            // Add data to VBOs
            float[] verticesData = GetDataFromVertexList();
            GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(verticesData.Length * sizeof(int)), verticesData, BufferUsageHint.StaticDraw);
            int[] indicesData = indices.ToArray<int>();
            GL.BufferData<int>(BufferTarget.ElementArrayBuffer, (IntPtr)(indicesData.Length * sizeof(int)), indicesData, BufferUsageHint.StaticDraw);

            // Check if everything was loaded correctly
            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (verticesData.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (indicesData.Length * sizeof(int) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            // Link attributes for shaders
            // Position
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            // Normal
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 8 * sizeof(float), 3 * sizeof(float));
            // UV
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            GL.BindVertexArray(0);
            triangles = indicesData.Length * 3;
        }

        private void SetUpBounds()
        {
            Vector3 min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            Vector3 max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            foreach (Vertex v in vertices)
            {
                if (v.position.X < min.X) min.X = v.position.X;
                else if (v.position.X > max.X) max.X = v.position.X;

                if (v.position.Y < min.Y) min.Y = v.position.Y;
                else if (v.position.Y > max.Y) max.Y = v.position.Y;

                if (v.position.Z < min.Z) min.Z = v.position.Z;
                else if (v.position.Z > max.Z) max.Z = v.position.Z;
            }

            bounds.min = min;
            bounds.max = max;

            bounds.ComputeCorners();
        }

        public void Render()
        {
            GL.BindVertexArray(VAO);
            GL.DrawElements(BeginMode.Triangles, triangles, DrawElementsType.UnsignedInt, 0);
        }
    }
}
