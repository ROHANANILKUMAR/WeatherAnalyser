<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WeatherAnalyser.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
		<link href="Scripts/CSS/Home.css" rel="stylesheet" />
        <div id="LargePanel">
			<div id="MainHead">Weather Analysing Platform</div>
			

			<div id="SubText">
				weather@kerala.in
			</div>
        </div>
		<div id="RightPanel">
			<div class="DarkRightHeader">
				SIGN UP
			</div>
			<div id="SignUpPanel">
				<ul style="list-style-type:none">
					<li class="SignUpEleLi">
						<div class="SignUpEle">
							Name			:  
								<asp:TextBox ID="Name" CssClass="Entry" runat="server" Height="17px" Width="375px"></asp:TextBox>
					
						</div>
					</li>
					<li class="SignUpEleLi">				
						<div class="SignUpEle">
							Location			:
							<asp:TextBox ID="Location" CssClass="Entry"  runat="server" Height="17px" Width="375px"></asp:TextBox>
						</div>
					</li>
					<li class="SignUpEleLi">
						<div class="SignUpEle">
							Address			:
							<asp:TextBox ID="Address" CssClass="Entry"  runat="server" Width="375px"></asp:TextBox>
						</div>
					</li>
					<li class="SignUpEleLi">
						<div class="SignUpEle">
							Phone No.		:
							<asp:TextBox ID="PhNo" CssClass="Entry"  runat="server" Height="16px" Width="375px"></asp:TextBox>
						</div>
					</li>
					<li class="SignUpEleLi">
						<div class="SignUpEle">
							Serial No.		:
							<asp:TextBox ID="Ser" CssClass="Entry"  runat="server" Height="17px" Width="375px"></asp:TextBox>
						</div>
						</li>
				</ul>
				<div id="signupbtn">
					<asp:Button ID="Sign_Up" runat="server" Text="Sign Up" BackColor="#00979D" BorderStyle="None" ForeColor="White"  Margin="100px" OnClick="Sign_Up_Click"/>
				</div>
			</div>
		</div>
		<div id="ContentPanel">
			<div class="Header">
				What is this?
			</div>
			<div class="Content">
				<p>			
				We want to change the world, make it a better and safer place. Our team aims to collect weather information and analyse it to predict possible disasters and report it to the navy/military for taking further actions. If we take the case of Kerala flood, it destroyed the life of many people, it eradicated agriculture, etc. Let no more disaster may befall on us. Choose a smart way, then a smart life will follow you.</p>
				<p>
In a developing country like India small contribution in the field of science leads to big changes both in the field of technology and the life of the people. A small carelessness may take away the lives of many. So here we have a WEATHER ANALYSER to sense the weather conditions and act accordingly. This technology is made after the Kerala flood conditions. If the action was taken a bit early many more could be saved. When the electricity and the communication facilities were lost, nobody knew where exactly the people were. This created difficulty for the authorities concerned with the rescue operations to do their duty.</p>
<p>
The people did not have the emergency number saved in their phone. Thus they couldn’t communicate with the rescuers during the flood. After when this weather analyser come into being, the message will be sent to the rescuers automatically by the machine. Hence this weather analyser is a life saver. 
</p>
			</div>
			<div class="Header">
				Instructions
				</div>
				<div class="Content">
					<p>Logon to our website and fill in your details. Click sign up to send your data to the servers.</p>
<p>The green light on your device will light up. This shows that the server has accepted your request to connect.</p>
<p>From this on your device will start analysing the weather around you. If it raises a red flag(not literally), it will signal the control room about the bad weather and then keep a log file. The rescue team will be there for you in a jiffy.</p>
<p>PS. Provide a working phone number.</p>

				
			</div>
			<div class="Header">
				Credits
				</div>
				<div class="Content">
					<p>This platform is brought to you by Rohan and Karthik who aims to achieve full automation in the world.</p>
				</div>
			
		</div>
		
				<div id="RightBottomPanel">
					<div class="DarkRightHeader">
			Credits
			</div>
					<div id="Credit">
					<img src="Image%20Files/Rohan.jpg" height="200" width="200" />
						<img src="Image%20Files/Karthik.jpeg" width="200" height="200" />

					</div>
					<div id="CreditName">
						<p>ROHAN ANILKUMAR AND KARTHIK KRISHNAN</p>
						<p>Computer Science students of KV EZHIMALA Class 11</p>
						</div>
						</div>
		
			
		
		<div id="BottomPanel">
			<p>Web Site Created By</p>
			<p>ROHAN ANILKUMAR</p>
			<p>&</p>
			<p>KARTHIK KRISHNAN</p>
		</div>
    </form>
</body>
</html>
 