using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    enum NodeState
    {
        FAILED,
        SUCCESS,
        RUNNING
    }
    abstract class TreeNode
    {
        protected TreeNode pNode;
        public TreeNode PNode { get { return pNode; } set { pNode = value; } }
        protected NodeState nState;
        public NodeState NState { get { return nState; } }

        public virtual TreeNode Enter()
        {
            nState = NodeState.RUNNING;
            return this;
        }
        public abstract TreeNode Update();
        public abstract void Exit();
    }
    /// <summary>
    /// 复合节点,拥有多个子节点
    /// </summary>
    class CompositeNode : TreeNode
    {
        protected List<TreeNode> childNodes;

        public void AddChildNode(TreeNode treeNode)
        {
            if (childNodes == null)
                childNodes = new List<TreeNode>();
            childNodes.Add(treeNode);
            treeNode.PNode = this;
        }
        public void RemoveChildNode(TreeNode treeNode)
        {
            childNodes.Remove(treeNode);
        }

        public override TreeNode Enter()
        {
            return base.Enter();
        }
        public override TreeNode Update()
        {
            return this;
        }
        public override void Exit()
        {
        }
    }
    /// <summary>
    /// 修饰节点,只有一个子节点
    /// </summary>
    class DecorateNode : TreeNode
    {
        protected TreeNode childNode;
        public void SetChildNode(TreeNode treeNode)
        {
            childNode = treeNode;
            treeNode.PNode = this;
        }
        public void RemoveChildNode()
        {
            childNode = null;
        }

        public override TreeNode Enter()
        {
            return base.Enter();
        }
        public override TreeNode Update()
        {
            return this;
        }
        public override void Exit()
        {
        }
    }
    /// <summary>
    /// 行为节点,没有子节点
    /// </summary>
    class ActionNode : TreeNode
    {
        public override TreeNode Enter()
        {
            return base.Enter();
        }
        public override TreeNode Update()
        {
            return this;
        }
        public override void Exit()
        {
        }
    }
    /// <summary>
    /// 顺序节点,依次执行子节点
    /// </summary>
    class SequenceNode : CompositeNode
    {
        int curRunningNodeIndex = 0;

        public override TreeNode Enter()
        {
            TreeNode runningNode = null;
            runningNode = base.Enter();
            if (childNodes != null)
            {
                curRunningNodeIndex = 0;
                runningNode = childNodes[0].Enter();
            }
            else
            {
                nState = NodeState.SUCCESS;
            }
            return runningNode;
        }
        public override TreeNode Update()
        {
            TreeNode runningNode = null;
            runningNode = base.Update();
            if (childNodes[curRunningNodeIndex].NState == NodeState.RUNNING)
            {
                runningNode = childNodes[curRunningNodeIndex].Update();
            }
            else
            {
                childNodes[curRunningNodeIndex].Exit();
                curRunningNodeIndex++;
                if (curRunningNodeIndex < childNodes.Count)
                    runningNode = childNodes[curRunningNodeIndex].Enter();
                else
                {
                    nState = NodeState.SUCCESS;
                }
            }
            return runningNode;
        }
    }

    class UntilSuccess : DecorateNode
    {
        public override TreeNode Enter()
        {
            TreeNode runningNode = null;
            runningNode = base.Enter();
            if (childNode != null)
            {
                runningNode = childNode.Enter();
            }
            else
            {
                nState = NodeState.SUCCESS;
            }
            return runningNode;
        }
        public override TreeNode Update()
        {
            TreeNode runningNode = null;
            runningNode = base.Update();
            switch (childNode.NState)
            {
                case NodeState.RUNNING:
                    runningNode = childNode.Update();
                    break;
                case NodeState.SUCCESS:
                    childNode.Exit();
                    nState = NodeState.SUCCESS;
                    break;
                case NodeState.FAILED:
                    runningNode = childNode.Enter();
                    nState = NodeState.RUNNING;
                    break;
            }
            return runningNode;
        }
    }

    class TreeManager
    {
        protected TreeNode rootNode;
        protected TreeNode curRunningNode;

        protected List<TreeNode> nodes;
        public void AddNode(TreeNode treeNode)
        {
            if (nodes == null)
                nodes = new List<TreeNode>();
            nodes.Add(treeNode);
        }
        public void RemoveNode(TreeNode node)
        {
            nodes.Remove(node);
        }

        public TreeManager(TreeNode rootNode)
        {
            this.rootNode = rootNode;
            curRunningNode = this.rootNode.Enter();
        }

        public void Update()
        {
            if (curRunningNode.NState != NodeState.RUNNING)
            {
                if (curRunningNode == rootNode)
                {
                    curRunningNode = rootNode.Enter();
                }
                else
                {
                    curRunningNode = curRunningNode.PNode;
                }
            }
            curRunningNode = curRunningNode.Update();
        }
    }
}
