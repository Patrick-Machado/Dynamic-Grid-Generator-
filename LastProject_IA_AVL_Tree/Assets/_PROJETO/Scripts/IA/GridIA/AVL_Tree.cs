using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVL_Tree : MonoBehaviour
{
    void start()
    {
        //AVL tree = new AVL();
        //tree.Add(5);
        //tree.Add(3);
        //tree.Add(7);
        //tree.Add(2);
        //tree.Delete(7);
        //tree.DisplayTree();
    }
}
public class AVL
{
    public class Node_AVL_Tree
    {
        public int data;
        public Node_AVL_Tree left;
        public Node_AVL_Tree right;
        public Edge_IA Edge_From_Grid;
        public Node_AVL_Tree(/*int data, */ Edge_IA e)
        {
            this.data = e.GetEdgeAvlID();
            this.Edge_From_Grid = e;
        }
    }
    Node_AVL_Tree root;
    public AVL()
    {
    }
    public void Add(/*int data,*/ Edge_IA e)
    {
        Node_AVL_Tree newItem = new Node_AVL_Tree(/*data,*/ e);
        if (root == null)
        {
            root = newItem;
        }
        else
        {
            root = RecursiveInsert(root, newItem);
        }
    }
    private Node_AVL_Tree RecursiveInsert(Node_AVL_Tree current, Node_AVL_Tree n)
    {
        if (current == null)
        {
            current = n;
            return current;
        }
        else if (n.data < current.data)
        {
            current.left = RecursiveInsert(current.left, n);
            current = balance_tree(current);
        }
        else if (n.data > current.data)
        {
            current.right = RecursiveInsert(current.right, n);
            current = balance_tree(current);
        }
        return current;
    }
    private Node_AVL_Tree balance_tree(Node_AVL_Tree current)
    {
        int b_factor = balance_factor(current);
        if (b_factor > 1)
        {
            if (balance_factor(current.left) > 0)
            {
                current = RotateLL(current);
            }
            else
            {
                current = RotateLR(current);
            }
        }
        else if (b_factor < -1)
        {
            if (balance_factor(current.right) > 0)
            {
                current = RotateRL(current);
            }
            else
            {
                current = RotateRR(current);
            }
        }
        return current;
    }
    public void Delete(int target)
    {//and here
        root = Delete(root, target);
    }
    private Node_AVL_Tree Delete(Node_AVL_Tree current, int target)
    {
        Node_AVL_Tree parent;
        if (current == null)
        { return null; }
        else
        {
            //left subtree
            if (target < current.data)
            {
                current.left = Delete(current.left, target);
                if (balance_factor(current) == -2)//here
                {
                    if (balance_factor(current.right) <= 0)
                    {
                        current = RotateRR(current);
                    }
                    else
                    {
                        current = RotateRL(current);
                    }
                }
            }
            //right subtree
            else if (target > current.data)
            {
                current.right = Delete(current.right, target);
                if (balance_factor(current) == 2)
                {
                    if (balance_factor(current.left) >= 0)
                    {
                        current = RotateLL(current);
                    }
                    else
                    {
                        current = RotateLR(current);
                    }
                }
            }
            //if target is found
            else
            {
                if (current.right != null)
                {
                    //delete its inorder successor
                    parent = current.right;
                    while (parent.left != null)
                    {
                        parent = parent.left;
                    }
                    current.data = parent.data;
                    current.right = Delete(current.right, parent.data);
                    if (balance_factor(current) == 2)//rebalancing
                    {
                        if (balance_factor(current.left) >= 0)
                        {
                            current = RotateLL(current);
                        }
                        else { current = RotateLR(current); }
                    }
                }
                else
                {   //if current.left != null
                    return current.left;
                }
            }
        }
        return current;
    }
    public Node_AVL_Tree Find(int key)
    {
        if(root == null) { return null; }
        Node_AVL_Tree n = Find(key, root);
        if(n == null) { return null; }
        if (/*Find(key, root).data*/ n.data == key)
        {
            Debug.Log(key + " was found!");
            return n;//Find(key, root);
        }
        else
        {
            Debug.Log("Nothing found!");
            return null;
        }
    }
    private Node_AVL_Tree Find(int target, Node_AVL_Tree current)
    {
        if(current == null) { return null; }
        if (target < current.data)
        {
            if (target == current.data)
            {
                return current;
            }
            else
                return Find(target, current.left);
        }
        else
        {
            if (target == current.data)
            {
                return current;
            }
            else
                return Find(target, current.right);
        }

    }
    public void DisplayTree()
    {
        if (root == null)
        {
            Debug.Log("Tree is empty");
            return;
        }
        Debug.Log("--------------------------TREE--------------------------------");
        InOrderDisplayTree(root);
        Debug.Log("--------------------------------------------------------------");
    }
    private void InOrderDisplayTree(Node_AVL_Tree current)
    {
        if (current != null)
        {
            InOrderDisplayTree(current.left);
            Debug.Log("("+ current.data + ") ");
            InOrderDisplayTree(current.right);
        }
    }
    private int max(int l, int r)
    {
        return l > r ? l : r;
    }
    private int getHeight(Node_AVL_Tree current)
    {
        int height = 0;
        if (current != null)
        {
            int l = getHeight(current.left);
            int r = getHeight(current.right);
            int m = max(l, r);
            height = m + 1;
        }
        return height;
    }
    private int balance_factor(Node_AVL_Tree current)
    {
        int l = getHeight(current.left);
        int r = getHeight(current.right);
        int b_factor = l - r;
        return b_factor;
    }
    private Node_AVL_Tree RotateRR(Node_AVL_Tree parent)
    {
        Node_AVL_Tree pivot = parent.right;
        parent.right = pivot.left;
        pivot.left = parent;
        return pivot;
    }
    private Node_AVL_Tree RotateLL(Node_AVL_Tree parent)
    {
        Node_AVL_Tree pivot = parent.left;
        parent.left = pivot.right;
        pivot.right = parent;
        return pivot;
    }
    private Node_AVL_Tree RotateLR(Node_AVL_Tree parent)
    {
        Node_AVL_Tree pivot = parent.left;
        parent.left = RotateRR(pivot);
        return RotateLL(parent);
    }
    private Node_AVL_Tree RotateRL(Node_AVL_Tree parent)
    {
        Node_AVL_Tree pivot = parent.right;
        parent.right = RotateLL(pivot);
        return RotateRR(parent);
    }
}