using System;

public class UserDomain
{
	public string userId;
	public string password;
	public int levelOfExpertise;
	public int speed;

	public UserDomain ()
	{
		this.userId = "defaultUser";
		this.password = "";
		this.speed = 10;
		this.levelOfExpertise = 1;
	}
}