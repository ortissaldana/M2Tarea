using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearPiramide : MonoBehaviour
{
    private class MyVector3
    {
        /* Creating a class called MyVector3 that contains 3 double values (x, y, z). */
        public double x;
        public double y;
        public double z;
       
        /// <param name="MyVector3">Tipo del vector.</param>
        /// Los valores negados del objeto MyVector3 original.
        public static MyVector3 operator -(MyVector3 vector)
        {
            return new MyVector3(-vector.x, -vector.y, -vector.z);
        }

        public MyVector3(double x, double y, double z)
        {
            /*Asignación de los valores de los parámetros a las variables de la clase.*/
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static explicit operator Vector3 (MyVector3 vector)
        {
            return new Vector3((float)vector.x, (float)vector.y, (float)vector.z);
        }
        /// Multiplica una matriz de 4x4 por una matriz de 4x1 y luego establece los valores x, y y z del punto en el
        /// primeros tres valores de la matriz 4x1 resultante
        /// <param name="MyVector3">A class that contains 3 double values (x, y, z)</param>
        public void Trasladar(MyVector3 vector)
        {
            double[][] matriz = new double[4][];
            matriz[0] = new double[] { 1, 0, 0, vector.x };
            matriz[1] = new double[] { 0, 1, 0, vector.y };
            matriz[2] = new double[] { 0, 0, 1, vector.z };
            matriz[3] = new double[] { 0, 0, 0, 1 };
            double[][] punto = new double[4][];
            punto[0] = new double[] { x };
            punto[1] = new double[] { y };
            punto[2] = new double[] { z };
            punto[3] = new double[] { 1 };
            double[][] resultado = Multipica(matriz, punto);
            this.x = resultado[0][0];
            this.y = resultado[1][0];
            this.z = resultado[2][0];
        }
        /// rota el vector alrededor de un eje por un ángulo, utilizando un punto de pivote
        /// <param name="axis">x, y, or z</param>
        /// <param name="angle">el ángulo de rotación</param>
        /// <param name="MyVector3">Una clase que contiene las coordenadas x, y y z de un punto.</param>
        public void Rotation(string axis, double angle, MyVector3 pivote)
        {
            double[][] matriz = new double[4][];
            angle = Mathf.Deg2Rad * angle;
            Trasladar(-pivote);

            if (axis == "x")
            {
                matriz[0] = new double[] { 1, 0, 0, 0 };
                matriz[1] = new double[] { 0, Mathf.Cos((float)angle), -Mathf.Sin((float)angle), 0 };
                matriz[2] = new double[] { 0, Mathf.Sin((float)angle), Mathf.Cos((float)angle), 0 };
                matriz[3] = new double[] { 0, 0, 0, 1 };
            }
            if (axis == "y")
            {
                matriz[0] = new double[] { Mathf.Cos((float)angle), 0, Mathf.Sin((float)angle), 0 };
                matriz[1] = new double[] { 0, 1, 0, 0 };
                matriz[2] = new double[] { -Mathf.Sin((float)angle), 0, Mathf.Cos((float)angle), 0 };
                matriz[3] = new double[] { 0, 0, 0, 1 };
            }
            if (axis == "z")
            {
                matriz[0] = new double[] { Mathf.Cos((float)angle), -Mathf.Sin((float)angle), 0, 0 };
                matriz[1] = new double[] { Mathf.Sin((float)angle), Mathf.Cos((float)angle), 0, 0 };
                matriz[2] = new double[] { 0, 0, 1, 0 };
                matriz[3] = new double[] { 0, 0, 0, 1 };
            }
            double[][] point = new double[4][];
            point[0] = new double[] { x };
            point[1] = new double[] { y };
            point[2] = new double[] { z };
            point[3] = new double[] { 1 };
            double[][] resultado = Multipica(matriz, point);
            MyVector3 final = new MyVector3(resultado[0][0], resultado[1][0], resultado[2][0]);
            final.Trasladar(pivote);
            this.x = final.x;
            this.y = final.y;
            this.z = final.z;
        }
        /// La función toma dos matrices y las multiplica juntas
        /// <param name="matrix">la matriz a multiplicar</param>
        /// <param name="point">el punto a transformar</param>
        private double[][] Multipica(double[][] matrix, double[][] point)
        {
            int rows = matrix.Length;
            int cols = point[0].Length;
            double[][] resultado = new double[rows][];
            for (int r = 0; r < rows; r++)
            {
                resultado[r] = new double[cols];
                for (int c = 0; c < cols; c++)
                {
                    double suma = 0;
                    for (int k = 0; k < point.Length; k++)
                    {
                        suma += matrix[r][k] * point[k][c];
                    }

                    resultado[r][c] = suma;
                }
            }
            return resultado;
        }
    }
    private Vector3[] vertices;
    private int[] triangles;
    void Start()
    {
        MyVector3 pivote = new MyVector3(-1.812, -6.824, 5.247);
        Debug.Log("Vertex Q");
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        MyVector3 q = new MyVector3(-3.05, -6.824, 4.897);
        q.Rotation("y", -15, pivote);
        Debug.Log("Vertex P");
        MyVector3 p = new MyVector3(0.060, -6.824, 4.897);
        p.Rotation("y", -15, pivote);
        MyVector3 r = new MyVector3(-1.812, -6.824, 7.043);
        r.Rotation("y", -15, pivote);
        MyVector3 s = new MyVector3(-1.812, -4.129, 5.247);
        s.Rotation("y", -15, pivote);
        /* Creando una matriz de 12 vértices. */
        vertices = new Vector3[12];
        /* Crear una matriz de 12 enteros.*/
        triangles = new int[12];
        /* Asignación de los vértices a los triángulos.*/
        vertices[0] = (Vector3) q;
        vertices[1] = (Vector3) r;
        vertices[2] = (Vector3) s;
        vertices[3] = (Vector3)r;
        vertices[4] = (Vector3)s;
        vertices[5] = (Vector3)p;
        vertices[6] = (Vector3)p;
        vertices[7] = (Vector3)s;
        vertices[8] = (Vector3)q;
        vertices[9] = (Vector3)q;
        vertices[10] = (Vector3)r;
        vertices[11] = (Vector3)p;
        /* Asignación de los vértices a los triángulos. */
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 3;
        triangles[4] = 4;
        triangles[5] = 5;
        triangles[6] = 6;
        triangles[7] = 7;
        triangles[8] = 8;
        triangles[9] = 10;
        triangles[10] = 11;
        triangles[11] = 9;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        /*Cálculo de las normales*/
        mesh.RecalculateNormals();
    }
}