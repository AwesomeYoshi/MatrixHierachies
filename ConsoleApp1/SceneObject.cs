using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Raylib;
using rl = Raylib.Raylib;

namespace ConsoleApp1
{
    public class SceneObject
    {
        protected SceneObject parent = null;
        protected List<SceneObject> children = new List<SceneObject>();

        protected Matrix3 localTransform = new Matrix3();
        protected Matrix3 globalTransform = new Matrix3();

        public SceneObject Parent
        {
            get { return parent; }
        }

        public SceneObject()
        {
            if (parent != null)
            {
                parent.RemoveChild(this);
            }

            foreach (SceneObject so in children)
            {
                so.parent = null;
            }
        }

        public int GetChildCount()
        {
            return children.Count;
        }

        public SceneObject GetChild(int index)
        {
            return children[index];
        }

        public void AddChild(SceneObject child)
        {
            //Make sure it doesn't have a parent already
            Debug.Assert(child.parent == null);

            child.parent = this;
            children.Add(child);
        }

        public void RemoveChild(SceneObject child)
        {
            if( children.Remove(child)== true)
            {
                child.parent = null;
            }
        }

        public virtual void OnUpdate(float deltaTime)
        {

        }

        public virtual void OnDraw()
        {

        }

        public void Update(float deltaTime)
        {
            //Run OnUpdate behaviour
            OnUpdate(deltaTime);

            //Update children
            foreach(SceneObject child in children)
            {
                child.Update(deltaTime);
            }
        }

        public void Draw()
        {
            //Run OnDraw behaviour
            OnDraw();

            //Draw children
            foreach (SceneObject child in children)
            {
                child.Draw();
            }
        }
        public Matrix3 LocalTransform
        {
            get { return localTransform; }
        }

        public Matrix3 GlobalTransform
        {
            get { return globalTransform; }
        }
        
        void UpdateTransform()
        {
            if(parent != null)
            {
                globalTransform = parent.globalTransform * localTransform;
            }
            else
            {
                globalTransform = localTransform;
            }

            foreach(SceneObject child in children)
            {
                child.UpdateTransform();
            }
        }

        public void SetPosition(float x, float y)
        {
            localTransform.SetTranslation(x, y);
            UpdateTransform();
        }
        public void SetRotate(float radians)
        {
            localTransform.SetRotateZ(radians);
            UpdateTransform();
        }
        public void SetScale(float width, float height)
        {
            localTransform.SetScaled(width, height, 1);
            UpdateTransform();
        }
        public void Translate(float x, float y)
        {
            localTransform.Translate(x, y);
            UpdateTransform();
        }
        public void Rotate(float radians)
        {
            localTransform.RotateZ(radians);
            UpdateTransform();
        }
        public void Scale(float width, float height)
        {
            localTransform.Scale(width, height, 1);
            UpdateTransform();
        }
    }

    public class Matrix3
    {
        public float m1, m2, m3, m4, m5, m6, m7, m8, m9;
    
        public Matrix3(float a1, float a2, float a3, float b1, float b2, float b3, float c1, float c2, float c3)
        {
            m1 = a1; m2 = a2; m3 = a3;
            m4 = b1; m5 = b2; m6 = b3;
            m7 = c1; m8 = c2; m9 = c3;
        }
        public Matrix3()
        {
            m1 = 1; m2 = 0; m3 = 0;
            m4 = 0; m5 = 1; m6 = 0;
            m7 = 0; m8 = 0; m9 = 1;
        }

        public void Set(Matrix3 input)
        {
            m1 = input.m1;
            m4 = input.m4;
            m7 = input.m7;
            m2 = input.m2;
            m5 = input.m5;
            m8 = input.m8;
            m3 = input.m3;
            m6 = input.m6;
            m9 = input.m9;

        }
        
        public void SetRotateZ(double radians)
        {
            Matrix3 rz = new Matrix3((float)Math.Cos(radians), (float)Math.Sin(radians), 0,
                                     (float)-Math.Sin(radians), (float)Math.Cos(radians), 0,
                                     0, 0, 1);

            Set(rz);
        }
        public void RotateZ(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateZ(radians);
            Set(this * m);
        }
        public void SetScaled(float x, float y, float z)
        {
            m1 = x; m2 = 0; m3 = 0;
            m4 = 0; m5 = y; m6 = 0;
            m7 = 0; m8 = 0; m9 = z;
        }
        public void Scale(float x, float y, float z)
        {
            Matrix3 mx = new Matrix3();
            mx.SetScaled(x, y, z);
            Set(this * mx);
        }

        public void SetTranslation(float x, float y)
        {
            m7 = x; m8 = y; m9 = 1;
        }
        public void Translate(float x, float y)
        {
            m7 += x; m8 += y;
        }


        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs.m1 * rhs.m1 + lhs.m4 * rhs.m2 + lhs.m7 * rhs.m3,
                lhs.m2 * rhs.m1 + lhs.m5 * rhs.m2 + lhs.m8 * rhs.m3,
                lhs.m3 * rhs.m1 + lhs.m6 * rhs.m2 + lhs.m9 * rhs.m3,
                lhs.m1 * rhs.m4 + lhs.m4 * rhs.m5 + lhs.m7 * rhs.m6,
                lhs.m2 * rhs.m4 + lhs.m5 * rhs.m5 + lhs.m8 * rhs.m6,
                lhs.m3 * rhs.m4 + lhs.m6 * rhs.m5 + lhs.m9 * rhs.m6,
                lhs.m1 * rhs.m7 + lhs.m4 * rhs.m8 + lhs.m7 * rhs.m9,
                lhs.m2 * rhs.m7 + lhs.m5 * rhs.m8 + lhs.m8 * rhs.m9,
                lhs.m3 * rhs.m7 + lhs.m6 * rhs.m8 + lhs.m9 * rhs.m9);
        }
    }
}
