using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTreeNode {
	private string question;
	private string good_option;
	private string bad_option;
	private Dictionary<string, DTreeNode> branches;

	private const string good_option_default = "good";
	private const string bad_option_default = "bad";

	public DTreeNode (string question, string good_option = good_option_default, 
		string bad_option = bad_option_default){

		this.question = question;
		this.good_option = good_option;
		this.bad_option = bad_option;
		this.branches = new Dictionary<string, DTreeNode>();
	}

	public void AddGoodOption(string question, string good_option = good_option_default, 
		string bad_option = bad_option_default){

		branches.Add("good", new DTreeNode (question, good_option, bad_option));
	}

	public void AddBadOption(string question, string good_option = good_option_default, 
		string bad_option = bad_option_default){

		branches.Add("bad", new DTreeNode (question, good_option, bad_option));
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

	public DTreeNode GetBadOptionNode(){
		return branches["bad"];
	}

	public DTreeNode GetGoodOptionNode(){
		return branches["good"];
	}

	public bool IsLeaf(){
		return ((branches.Count == 0) && (bad_option == bad_option_default)
			&& (good_option == good_option_default));
	}
}
