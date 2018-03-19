using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTreeNode {
	private string question;
	private string good_option;
	private string bad_option;
	private LinkedList<DTreeNode> branches;

	public DTreeNode (string question, string good_option, string bad_option){
		this.question = question;
		this.good_option = good_option;
		this.bad_option = bad_option;
		this.branches = new LinkedList<DTreeNode>();
	}

	public void AddChild(string question, string good_option, string bad_option){
		branches.AddFirst (new DTreeNode (question, good_option, bad_option));
	}

	public DTreeNode GetChild(int i){
		foreach (DTreeNode d in branches)
			if (--i == 0)
				return d;
		return null;
	}

	public string GetQuestion(){
		return question;
	}

	public string GetGoodOption(){
		return good_option;
	}

	public string GetBadOption(){
		return bad_option;
	}

	public bool IsLeaf(){
		return (branches.Count == 0);
	}
}
