using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Helper
{
	
    public static Dictionary<string, string> FindSchool(string participant_id)
    {
        string school_code = participant_id.Substring(participant_id.Length - 1);
        Dictionary<string, string> school_data = new Dictionary<string,string>();
        switch (school_code)
        {
            case "s":
                school_data= new Dictionary<string, string>() 
                {
                    {"code", "s"},
                    {"name", "Stanford University"},
                    {"dept", "Stanford Counseling and Psychological Services"},
                    {"addr", "866 Campus Drive"},
                    {"tele", "650-723-3785"},
                    {"email", "ebridgeproject@stanford.edu"},
                    {"email_server", "ebridgeproject.stanford.edu"},
					{"test_server", "http://hmpweb.sph.umich.edu/ebridge"},
                };
                break;
            case "n":
                school_data = new Dictionary<string, string>() 
                {
                    {"code", "n"},
                    {"name", "University of Nevada, Reno"},
                    {"dept", "Counseling Services"},
                    {"addr", "866 Campus Drive"},
                    {"tele", "650-723-3785"},
                    {"email", "ebridge@unr.edu "},
                    {"email_server", "ebridge.unr.edu"},
					{"test_server", "http://hmpweb.sph.umich.edu/ebridge"},
                };
                break;
            case "i":
                school_data = new Dictionary<string, string>() 
                {
                    {"code", "i"},
                    {"name", "University of Iowa"},
                    {"dept", "University of Iowa Student Health"},
                    {"addr", "866 Campus Drive"},
                    {"tele", "650-723-3785"},
                    {"email", "ebridgeteam@uiowa.edu"},
                    {"email_server", "psych-ebridge@uiowa.edu"},
					{"test_server", "http://hmpweb.sph.umich.edu/ebridge"},
                };
                break;
            case "m":
                school_data = new Dictionary<string, string>() 
                {
                    {"code", "m"},
                    {"name", "University Michigan"},
                    {"dept", "UMich Counseling and Psychological Services"},
                    {"addr", "866 Campus Drive"},
                    {"tele", "650-723-3785"},
                    {"email", "ebridgeteam@umich.edu"},
                    {"email_server", "ebridge.umich.edu"},
					{"test_server", "http://hmpweb.sph.umich.edu/ebridge"},
                };
                break;
            default:
                throw new System.ArgumentException("School code did not match");
        }
        return school_data;
    }
	public static string ConsentText(string participant_id)
    {
        string school_code = participant_id.Substring(participant_id.Length - 1);
        string consent_string;
        switch (school_code)
        {
            case "s":
                consent_string = @"<CENTER><B style='font-size:16px'>Welcome to the Electronic Bridge to Mental Health (e-Bridge) for College Students Study</B></CENTER>

				<BR>&nbsp;<BR>

				Welcome to the <I>e</I>Bridge study of college student mental health! Our goal is to learn more about how to provide support to college students who are experiencing mental or emotional difficulties. If you participate in this study, you will be asked to complete a brief web survey, <I>which takes less than 5 minutes for most participants</I>. You will then receive automated feedback about your current emotional health as well as suggestions for types of support or services you may want to consider. Please read more information about the study below, and then indicate at the bottom of the screen whether you are willing to participate in this study.

				<BR>&nbsp;<BR>

				<DIV style='border:1px solid gray;padding:15px;width:588px;height:400px;overflow:auto'>

				<SPAN style='font-size:9px'>Study No: HUM00091681<BR>IRB: IRBMED<BR>Consent Approved On: 10/15/2014<BR>Project Approval Expires On: 10/14/2015</SPAN><P>

				<CENTER>
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px'>NAME OF STUDY AND RESEARCHERS</DIV><P>
                <B>STANFORD UNIVERSITY</B><P>
				<B>Research Consent Form</B><P>
                Protocol Director: Ronald Albucher<P>
                Protocol Title: Electronic Bridge to Mental health (e-Bridge) for College Students<P><P><P>
                *************
				</CENTER>
                <b>FOR QUESTIONS ABOUT THE STUDY, CONTACT:<br>
                   Ronald Albucher, M.D.<BR>
                   866 Campus Drive<BR>
                   Stanford, CA 94305<BR>
                   650-725-1357<BR>
                   albucher@stanford.edu</b><P>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>DESCRIPTION</DIV><P>

				You are invited to participate in a research study on e-Bridge, a service
                to link students with depression or other emotional difficulties to services or resources. We 
                hope to learn about how to best implement e-Bridge and to find out if it improves linkages to 
                services or resources. Your participation is voluntary--your decision of whether to participate 
                will not affect your standing at this university in any way, and you will not be penalized in any 
                way if you decide not to participate. You may discontinue participation at any time.<P>

                You will be asked to complete an online survey if you agree to participate. Depending on the
                questions you are asked, this survey will take anywhere from approximately 3 to 12 minutes. 
                You will be asked questions about your race, ethnicity, thoughts, behaviors, and moods. You 
                may also be asked questions about any mental health services you may or may not have 
                received and your views about such services.<P>

                Regardless of whether you participate, you will be automatically entered into a random 
                drawing for gift cards.<P>

                Some students will be invited to continue on to Part Two. These students will have an
                opportunity to view personalized feedback regarding their survey responses. Approximately 
                one-half of these students will have the option to exchange online messages with a 
                professional counselor. The counselor does not provide treatment and is unable to respond 
                rapidly to online messages requiring immediate assistance for risk or harm to self or others.  
                All students in Part Two will be presented with information about local services.<P>

                For those participating in Part Two, we will contact you again four weeks later and
                approximately five to six months later for online follow-up surveys (lasting 5-12 minutes) 
                about how you are doing and any services you have obtained. All those invited to participate 
                in these surveys will receive a $10 gift card; with an additional $15 for completing the four 
                week survey and an additional $25 for completing the five to six month survey.<P>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>RISKS AND BENEFITS</DIV><P>

				The risks associated with this study are: some of the questions
                will ask you about sensitive or personal information such as your emotional health. These 
                questions might make you feel uncomfortable or anxious. At the end of the survey you will 
                receive a list of resources on campus that can provide help and support if ever needed.  
                If responding to any questions makes you feel distressed, we urge you to call any of the resources listed.<P>

                The benefits which may reasonably be expected to result from this study are: students may 
                learn important information about available services. Some students may also link to needed 
                services as a result of study participation. We expect this research to be used to gain an 
                understanding of how to best provide links and resources for college students. We cannot and 
                do not guarantee or promise that you will receive any benefits from this study.<P>

                Your decision whether or not to participate in this study will not affect your student status or
                health care.<P>

                <DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>TIME INVOLVEMENT</DIV><P>
                
                Your participation in this experiment will take approximately 3 to 12
                minutes, depending on the questions you are asked. For those participating in Part Two, we 
                will contact you again four weeks later and approximately five to six months later for online 
                follow-up surveys each lasting 5-12 minutes.<P>

                <DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>PAYMENTS</DIV><P>
                
                You will be entered into a drawing to win one of ten $100 gift cards. All students
                invited to participate in Part II of the study will receive a $10 gift card; with an additional $15 
                for completing the four week survey and an additional $25 for completing the five to six month 
                survey.<P>

                Payments may only be made to U.S. citizens, legal resident aliens, and those who have a work
                eligible visa. You may need to provide your social security number to receive payment.<P>

                <DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>PARTICIPANT'S RIGHTS</DIV><P>
                
                If you have read this form and have decided to participate in this
                project, please understand your participation is voluntary and you have the right to withdraw 
                your consent or discontinue participation at any time without penalty or loss of benefits to 
                which you are otherwise entitled.<P>

                The results of this research study may be presented at scientific or professional meetings or
                published in scientific journals. However, your identity will not be disclosed. You have the 
                right to refuse to answer particular questions.<P>
				
                
				</DIV>

				<DIV style='padding-top:5px;padding-bottom:20px'>If you wish to save or print a copy of this consent document for your own records, you can click <A href='../survey/question/CONSENT_STANFORD.pdf' target='_blank'><B>here</B></A> to view a printable version.</I></DIV>

				<TABLE width='600px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>
				<TR height='25px'><TD style='padding-right:8px'><INPUT type='radio' name='Y' id='S'></TD><TD><SPAN onclick=""document.getElementById('S').click()"" style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=""this.style.backgroundColor='yellow'"" onmouseout=""this.style.backgroundColor='white'""><B>YES</B>, I have read the information above and consent to participate in this study.</SPAN></TD></TR>
				<TR height='25px'><TD style='padding-right:8px'><INPUT type='radio' name='Y' id='N'></TD><TD><SPAN onclick=""document.getElementById('N').click()"" style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=""this.style.backgroundColor='yellow'"" onmouseout=""this.style.backgroundColor='white'""><B>NO</B>, I do not wish to participate in this study and understand that there is no penalty for not participating.</SPAN></TD></TR>
				<TR height='5px'><TD></TD></TR>
				</TABLE>

				<CENTER style='padding-top:25px;padding-bottom:15px'><IMG src='../image/next.png' border='0' onclick=""if (document.getElementById('S').checked){window.location.href='screen.aspx?p=0';} else if(document.getElementById('N').checked){alert('Thanks for checking out the study. You may now close your browser window.');return false;} else {alert('Please select -YES- before proceeding; or -NO- to quit the survey.');return false;}"" style='cursor:hand;cursor:pointer'></CENTER>";

                break;
            case "n":
                consent_string = @"<CENTER><B style='font-size:16px'>Welcome to the Electronic Bridge to Mental Health (e-Bridge) for College Students Study</B></CENTER>

				<BR>&nbsp;<BR>

				Welcome to the <I>e</I>Bridge study of college student mental health! Our goal is to learn more about how to provide support to college students who are experiencing mental or emotional difficulties. If you participate in this study, you will be asked to complete a brief web survey, <I>which takes less than 5 minutes for most participants</I>. You will then receive automated feedback about your current emotional health as well as suggestions for types of support or services you may want to consider. Please read more information about the study below, and then indicate at the bottom of the screen whether you are willing to participate in this study.

				<BR>&nbsp;<BR>

				<DIV style='border:1px solid gray;padding:15px;width:588px;height:400px;overflow:auto'>


				<CENTER>
				<B>THE UNIVERSITY OF NEVADA-RENO</B><P>
				<B>CONSENT TO BE PART OF A RESEARCH STUDY</B><P>
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px'><b>NAME OF STUDY AND RESEARCHERS</b></DIV><P>

				<B><u>NAME:</u> Electronic Bridge to Mental Health (e-Bridge) for College Students</B><P>
				<B><u>PRINCICPAL INVESTIGATOR: </u>Jacqueline Pistorello, Ph.D., University of Nevada-Reno</B><BR><a href='mailto:pistorel@unr.edu'>pistorel@unr.edu</a><BR>
				<i>This is a multi-site study and the primary Principal Investigator is Cheryl King, Ph.D. at the University of Michigan, <a href='mailto:kingca@umich.edu'>kingca@umich.edu</a></i><BR>
				<B><u>CO-INVESTIGATORS:</u></B> Stephen Chermack, Ph.D., Daniel Eisenberg, Ph.D.<BR>
				Inbal Nahum-Shani, Ph.D., and Kai Zheng, Ph.D., University of Michigan; Ronald Albucher, M.D., Stanford University; & William Coryell, M.D., University of Iowa<BR>
				<B><u>STUDY ID NUMBER: </u></B>659028<BR>
				<B><u>SPONSOR:</u></B> National Institute of Mental Health<P>
				</CENTER>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Introduction</DIV><P>
				
				Before you agree to be in this study, please take time to read this form. It explains why we are doing this study, and the procedures, risks, discomforts, benefits, and precautions involved.
				This form may use words you do not understand.  Please ask the researchers to explain anything that you do not understand.<P>
				Please be completely truthful about your eligibility to be in this study. If you are not truthful, you may harm yourself by being in the study.<P>
				You do not have to be in this study.  Your participation in voluntary.<P>
				Take as much time as you need to decide.  If you say yes now but change your mind, you may quit the study at any time.  Just let one of the researchers know you do not want to continue.<P>
				
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Why are we doing this study?</DIV><P>
				We are doing this study to find out how to best link college students who are psychologically distressed to potentially helpful services.<P>
				
				Researchers at the University of Michigan, University of Iowa, University of Nevada-Reno, and Stanford University are conducting research with college students to learn about <i>e</i>Bridge,
				a service to help link students with depression ot other emotional difficulties to services or resources that may be helpful.  We hope to learn about how to best implement <i>e</i>Bridge
				and to find out if it improves linkages to services or resources.  As the University of Michigan is the primary site, surveys, emails, and data analysis for the study occurs via the University of Michigan<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Why are we asking you to be in the study?</DIV><P>
				We are asking you to be in this study because you are enrolled at the University of Nevada, Reno and are 18 years of age or older.<P>
				
				A total of up to 7,400 UNR students will be recruited to complete this screening and up to 45,000 college students across all sites in the study.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>What happens if you agree to be in the study?</DIV><P>
				<b><u>Part 1:</u></b><br>
				If you agree to participate, we will ask you to complete an online survey that will take either take 2 to 5 minutes or 8 to 10 minutes, depending upon the number of questions included in your survey.
				All students will be asked questions about their age, race, ethnicity, thoughts, behaviors, and moods.  All students will be asked about their use of mental health services.  
				Some students will be asked additional questions about their emotions and behaviors, views about mental health services, and academic functioning.  The survey will require a total of 8 to 10 minutes for these students.
				We anticipate that approximately  5 to 10 percent of students will be presented these additional questions.  These students will be identified based on their responses to the first set of questions.
				They are students for whom we anticipate that the <i>e</i>Bridge intervention may be helpful.<P>

				<b><u>Part 2:</u></b><br>
				All students who complete the longer online survey will have an opportunity to view personalized feedback regarding their survey responses.  
				Approximately one-half of these students will be randomly selected to participate in the <i>e</i>Bridge intervention,
				which will present them with the option of exchanging online messages with a professional counselor at the University of Iowa about their personalized feedback , their concerns and/or the availability of resources in the campus community.
				For the randomization each student has approximately a 50-50 chance of of being assigned to the <i>e</i>Bridge group or the personalized feedback and information services group only.<P>
				
				For those participating in Part 2, we will contact you again four weeks later and approximately five to six months later for online follow-up surveys (lasting 5-12 minutes each) about 
				how you are doing and any services you have obtained.<P>
				
				Additionally we would like to ask you permission to access your academic records and to confirm that you have or have not accessed services at Counseling Services and Student Health
				Services at University of Nevada, Reno (see below).  These records will be used for research purposes only and will be kept confidential by the research team.<P>
				
				Some financial incentives for completion of online surveys will be provided. See below.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>How long will you be in the study?</DIV><P>
				The study will vary considerably in how long it will take.  It may only take 2-10 minutes if you only participate in the initial online survey,
				but may take up to a couple of hours if you participate in Part 2 with online contact with a counselor and completion of follow-up questionnaires 6 months after the initial contact.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>What happens if you do not want to be in the study?</DIV><P>
				Nothing will happen if you decide not be in this study.  Your participation is voluntary - your decision of whether or not to participate will not affect your standing at
				University of Nevada, Reno in any way, and you will not be penalized in any way if you decide not to participate. You may discontinue participation at any time.
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Is there any way being in this study could be bad for you?</DIV><P>
				If you agree to be in this study, you may be asked about sensitive or personal information such as your emotional health.
				These questions might make you feel uncomfortable or anxious.
				At the end of the survey you will receive a list of resources on campus that can provide help and support if ever needed.
				If responding to any questions makes you feel distressed, we urge you to call any of the resources listed.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>We being in this study help you in any way?</DIV><P>
				Being in this study may not help you but you may learn important information about available services.
				Some students may also link to needed services as a result of study participation.<BR>
				
				Benefits of doing research are not definite; however we hope to learn how to best provide links and resources for college students.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>How will we protect your private information and the information we collect about you?</DIV><P>
				We will treat your identity with professional standards of confidentiality and protect your private information to the extent allowed by law.
				We will do this by taking several precautions.<P>
				
				We are in the process of obtaining a Certificate of Confidentiality from the Federal Government.
				This will help protect your privacy by allowing us to refuse to release your name or other information outside of the research study, even by a court order.
				The Certificate does not prevent you or a member of your family from releasing information about yourself or your involvement in this study.<P>

				We will maintain a secure data environment using Secure Sockets Layer (SSL) encryption technology. 
                Although you will be providing personal information transferred through a study website, we will never link this information to any of the study data (from the surveys and from the correspondence via the website).
                We will only use identifying information (name  and email address) to recruit participants for participation, contact students who win gift cards in the random drawing, to obtain records (see additional information below), or to contact you for optional participation in future phases of this project.
				We will keep your contact information on file after the six month assessment in order to keep you updated on the progress of the study and to contact you for possible participation in future phases of the study.
				Your contact information will be stored in a password protected data file, which will only be available to the research staff.<P>
				
				The data from this study, without any identifiable information, will be retained in a secure repository for future research purposes.
				Records will be kept confidential to the extent provided by federal, state, and local law.
				However, the UNR and University of Michigan Institutional Review Boards, the sponsor of the study, or the university and government officials responsible for monitoring this study may inspect these records.<P>
				
				Should you accidentally leave a session open on a computer that may be viewed by others, the computer will automatically log off.  
				After 10 minutes of idle time, you will receive a notification that the session will be logged off in 1 minutes unless you click to continue the session.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Who will know that you are in this study and who will have access to the information we collect about you?</DIV><P>
				The researchers at the University of Michigan and at the University of Nevada, Reno, the US Department of Health and Human Services (DHHS), 
				the University of Nevada, Reno Social Behavioral Institutional Review Board, the University of Michigan Institutional Review Board, 
				or university and government officials responsible for monitoring this study, and the National Institute of Health may inspect your study records.<P>
				
				There are exceptions to confidentiality.  By law, if we know your identity (if you reveal that information to an online counselor) we must notify the authorities if 
				we find or suspect child abuse, elder abuse, a high intent to harm yourself or others, or determine you have an infectious disease and have not reported it yourself.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Will it cost you anything to be in the study?</DIV><P>
				There will be no costs to you to be in the study?<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Will you be paid for being in this study?</DIV><P>
				There will be financial incentives:<br>
				<b>1)</b> Regardless of whether you complete the initial survey or not, you will be automatically entered into a random drawing for gift cards (ten $100 gift cards at each participating institution).
				The odds of winning one of these gift cards at UNR in approximately 1 out of 700.  The drawing will be conducted at the University of Michigan in the fall of each year.
				Winners will be notified by email within three days and asked to provide their names and mailing addresses so prizes can be mailed.
				You may opt out of the drawing by contacting the researchers by email.<P>
				
				<b>2)</b> For students participating in Part 2, all those invited to participate in follow-up surveys will receive a $10 gift card, 
				with an additional $15 for completing the four week survey and an additional $25 for completing the five to six month survey.
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>What happens if you agree to be in the study now, but change your mind later?</DIV><P>
				You do not have to stay in the study.  You may withdraw from the study at any time by sending an email to Dr. Jacqueline Pistorello at <a href='mailto:pistorel@unr.edu'>pistorel@unr.edu</a> 
				or by calling her at 775-846-5540.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>What if the study changes while you are in it?</DIV><P>
				If anything about the study changes or if we use your data in a different way, we will tell you and ask if you want to stay in the study.
				We will also tell you about any important new information that may affect your willingness to stay in the study.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Who can you contact if you have questions about the study or want to report an injury?</DIV><P>
				At any time, if you have questions about this study or wish to report an injury that may be related to your participation in this study,
				you may contact Dr. Jacqueline Pistorello at <a href='mailto:pistorel@unr.edu'>pistorel@unr.edu</a> or 775-846-5540.
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Who can you contact if you want to ask about your rights as a research participant?</DIV><P>
				You may ask about your rights as a research participant or talk (anonymously if you choose) to the University of Nevada, Reno Social Behavioral Institutional Review Board by 
				calling (775) 327-2368 or sending a note from the <i>Contact Us</i> page of this website: <a href='http://www.unr.edu/research-integrity'>http://www.unr.edu/research-integrity.</a><P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:left'>Do the researchers have monetary interests tied to the study?</DIV><P>
				The researchers and/or their families do not have any monetary interests tied to this study.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'><b>PERMISSION TO UNR TO RELEASE INFORMATION</b></DIV><P>
				I hereby authorize the investigators in this study, to obtain information regarding my academic record at UNR,
				including enrollment status, class load, GPA, demographic information, and high school information, starting today and for the next academic year. 
				For example, if I join the study in the Fall semester of 2014, the investigators would have permission to collect these data through Fall of 2015.<P>
				
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'><b>PERMISSION TO UNR COUNSELING SERVICES AND STUDENT HEALTH CENTER</b></DIV><P>
				I hereby authorize the investigators in this study, to obtain information regarding my attendance of individual or group counseling sessions or attendance of a medication 
				management session with a psychiatrist at UNR Counseling Services and UNR Student Health Center during the study.<P>
				
				</DIV>
				<P>
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'><b>AGREEMENT TO BE IN THE STUDY</b></DIV><P>
				<DIV style='padding-top:5px;padding-bottom:20px'>If you wish to save or print a copy of this consent document for your own records, you can click <A href='../survey/question/CONSENT_UNR.pdf' target='_blank'><B>here</B></A> to view a printable version.</I></DIV><P>
				
				
				By clicking yes you are saying:
				<ul>
					<li>You agree to be in this study</li>
					<li>We talked with you about the information in this document and answered all of you questions</li>
				</ul>
				
				You know that:
				<ul>
					<li>You may skip questions you do not want to answer.</li>
					<li>You may stop participating in the research at any time.</li>
					<li>You may call the University office in charge of research at (775) 327-2368 if you have any questions about the study or about your rights.</li>
				</ul><P>
				
				<TABLE width='600px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>
				<TR height='25px'><TD style='padding-right:8px'><INPUT type='radio' name='Y' id='S'></TD><TD><SPAN onclick=""document.getElementById('S').click()"" style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=""this.style.backgroundColor='yellow'"" onmouseout=""this.style.backgroundColor='white'""><B>YES</B>, I have read the information above and consent to participate in this study.</SPAN></TD></TR>
				<TR height='25px'><TD style='padding-right:8px'><INPUT type='radio' name='Y' id='N'></TD><TD><SPAN onclick=""document.getElementById('N').click()"" style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=""this.style.backgroundColor='yellow'"" onmouseout=""this.style.backgroundColor='white'""><B>NO</B>, I do not wish to participate in this study and understand that there is no penalty for not participating.</SPAN></TD></TR>
				<TR height='5px'><TD></TD></TR>
				</TABLE>

				<CENTER style='padding-top:25px;padding-bottom:15px'><IMG src='../image/next.png' border='0' onclick=""if (document.getElementById('S').checked){window.location.href='screen.aspx?p=0';} else if(document.getElementById('N').checked){alert('Thanks for checking out the study. You may now close your browser window.');return false;} else {alert('Please select -YES- before proceeding; or -NO- to quit the survey.');return false;}"" style='cursor:hand;cursor:pointer'></CENTER>";
                break;
            case "i":
				consent_string = @"<CENTER><B style='font-size:16px'>Welcome to the Electronic Bridge to Mental Health (e-Bridge) for College Students Study</B></CENTER>

				<BR>&nbsp;<BR>

				Welcome to the <I>e</I>Bridge study of college student mental health! Our goal is to learn more about how to provide support to college students who are experiencing mental or emotional difficulties. If you participate in this study, you will be asked to complete a brief web survey, <I>which takes less than 5 minutes for most participants</I>. You will then receive automated feedback about your current emotional health as well as suggestions for types of support or services you may want to consider. Please read more information about the study below, and then indicate at the bottom of the screen whether you are willing to participate in this study.

				<BR>&nbsp;<BR>

				<DIV style='border:1px solid gray;padding:15px;width:588px;height:400px;overflow:auto'>

				<SPAN style='font-size:9px'>Study No: HUM00091681<BR>IRB: IRBMED<BR>Consent Approved On: 10/15/2014<BR>Project Approval Expires On: 10/14/2015</SPAN><P>

				<CENTER>
				<B>THE UNIVERSITY OF IOWA<P>
				<B>CONSENT TO BE PART OF A RESEARCH STUDY</B><P>
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px'>NAME OF STUDY AND RESEARCHERS</DIV><P>

				<B>Electronic Bridge to Mental Health (e-Bridge) for College Students</B><P>
				<B>Site Investigator at University of Iowa:</B> William Coryell, MD<BR><BR>
				<B>Principal Investigator:</B> Cheryl A. King, Ph.D.<BR>
				<B>Co-Investigators:</B> Stephen Chermack, Ph.D., Daniel Eisenberg, Ph.D.<BR>
				Inbal Nahum-Shani, Ph.D., Kai Zheng, Ph.D.<BR>
				<B>Funding Provided by:</B> Department of Health and Human Services, National Institute of Mental Health<P>
				</CENTER>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>GENERAL INFORMATION</DIV><P>

				Researchers at the University of Michigan, University of Iowa, University of Nevada-Reno, and Stanford University are conducting research with college students to learn about <i>e</i>Bridge,
				a service to help link students with depression ot other emotional difficulties to services or resources that may be helpful.  We hope to learn about how to best implement <i>e</i>Bridge
				and to find out if it improves linkages to services or resources.  As the University of Michigan is the primary site, surveys, emails, and data analysis for the study occurs via the University of Michigan<P>

				<b>What is this study and why are we asking you to participate?</b><P>

                We invite you to participate in a research study.  The purpose of the study is to conduct research with college students to 
                learn about <i>e</i>Bridge, a service to help link students with depression or other emotional difficulties to services or resources 
                that may be helpful. We hope to learn about how to best implement <i>e</i>Bridge and to find out if it improves linkages to services or resources.<P>

				We are inviting you to be in this study because you are 18 years or older and attend the University of Iowa.
                We obtained your name and email address from the University of Iowa's Office of the Registrar.
                We expect approximately 13,640 people will take part in this study at the University of Iowa and 45,188 across all sites<P>

				<b>What is involved if I participate in this study?</b><P>
				<b>Part 1:</b><br>
				If you agree to participate, we will ask you to complete an online survey that will take either take 2 to 5 minutes or 8 to 10 minutes, depending upon the number of questions incuded in your survey.
				All students will be asked questions about their age, race, ethnicity, thoughts, behaviors, and moods.  All students will be asked about their use of mental health services.  
				Some students will be asked additional questions about their emotions and behaviors, views about mental health services, and academic functioning.  The survey will require a total of 8 to 10 minutes for these students.
				We anticipate that approximately  5 to 10 percent of students will be presented these additional questions.  These students will be identified based on their responses to the first set of questions.
				They are students for whom we anticipate that the <i>e</i>Bridge intervention may be helpful.<P>

				<b>Part 2:</b><br>
				All students who complete the longer online survey will have an opportunity to view personalized feedback regarding their survey responses.  
				Approximately one-half of these students will be randomly selected to participate in the <i>e</i>Bridge intervention,
				which will present them with the option of exchanging online messages with a professional counselor at the University of Iowa about their personalized feedback , their concerns and/or the availability of resources in the campus community.
				For the randomization each student has approximately a 50-50 chance of of being assigned to the <i>e</i>Bridge group or the personalized feedback and information services group only.<P>

				<b>Personalized Feedback Group:</b><br>
				You will receive personalized feedback regarding your survey responses that will include information about local services available.<P>
				We will contact you again four weeks later and approximately five to six months later for online follow-up surveys (lasting 6-10 minutes) about how your are doing and any services you have obtained.<P>
				<b>Online Message Group:</b><br>
				You will receive personalized feedback regarding your survey responses that will include information about local services available and you will have the option to exchange online messages with professional counselor.<P>
				The counselor does not provide treatment and is unable to respond rapidly to online messages requiring immediate assistance for risk or harm to self or others.<P>
				We will contact you again four weeks later and approximately five to six months later for online follow-up surveys (lasting 6-10) about how you are doing and any services you have obtained.<P>
                Taking part in this research study is completely voluntary.
                If you decide not to be in this study, or if you stop participating at any time, you won't be penalized or lose any benefits for which you otherwise qualify.
                If you do not want to participate in this study click on the link at the end of this form to decline participation and any further email reminders.
                If you do not decline to participate you will be sent 3 email reminders to complete the survey spaced 3-4 days apart.
                
                <DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>CONFIDENTIALITY OF SUBJECT RECORDS</DIV><P>
                <b>How will my privacy and confidentiality be protected?</b><br>
                We will keep the information you provide confidential, however federal regulatory agencies and the University of Iowa Institutional Review Board (a committee that reviews and approves research studies) may inspect and copy records pertaining to this research. 
                If we write a report about this study we will do so in such a way that you cannot be identified. 
                The data from this study, without any identifiable information, will be retained in a secure repository for future research purposes.<P>

                We will do everything we can to protect your privacy. We have a Certificate of Confidentiality from the National Institute of Health for this study.\
                This protects us from having to release information that could be used to identify you.
                It allows us to refuse to disclose such information in any civil, criminal, administrative, legislative, or other proceeding, whether at the federal, state, or local level.
                It does not however, prevent you from choosing to disclose information that we obtain in this study to physicians or others.<P>

                The University of Michigan maintains a secure data environment using Secure Sockets Layer (SSL) encryption technology. 
                Although students are providing personal information transferred through a study website, we will never link this information to any of the study data (from the surveys and from the correspondence via the website).
                We will only use identifying information (name  and email address) to recruit you for participation, contact you if you win gift cards in the random drawing, to obtain records (see additional information below), or to contact you for optional participation in future phases of this project.<P>
                
                One limit to confidentiality is imminent risk for suicide or serious self-harm.
                If an on-line counselor makes the clinical judgment that you are at imminent risk for serious self-harm, that counselor is obligated to attempt to obtain your identity and location and transmit this information to emergency responders in an effort to preserve life.<P>

                We will keep your contact information on file after the six month assessment in order to keep you updated on the progress of the study and to contact you for possible participation in future phases of the study.
                Your contact information will be stored in a password protected data file, which will only be available to the research staff.<P>

                Should you accidentally leave a session open on a computer that may be viewed by others, the computer will automatically log-off.  
                After 10 minutes of idle time, you will receive a notification that the session will be logged off in 1 minute unless you click to continue the session.<P>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>INFORMATION ABOUT RISKS AND BENEFITS</DIV><P>
			    There are no known risks from being in this study, and you will not benefit personally.  
                However we hope that others may benefit in the future from what we learn as a result of this study. <P>

                Some of the questions will ask you about sensitive or personal information such as your emotional health. 
                These questions might make you feel uncomfortable or anxious. 
                At the end of the survey you will receive a list of resources on campus that can provide help and support if ever needed. 
                If responding to any questions makes you feel distressed, we urge you to call any of the resources listed.<P>
                
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>FINANCIAL INFORMATION</DIV><P>

                <b>Do I have to pay anything to participate in the study?</b><br>
				There are no costs to you for participating in this study.<P>

				<b>Will I be paid or given anything for taking part in this study?</b><br>
				Regardless of whether or not you participate, you wil be automatically entered into a random drawing for gift cards (ten $100 gift cards at each participating university).
				The drawing will be conducted at the University of Michigan on December 14, 2014.  Winners will be notified by email within three business days and asked to provide their names and mailing address so prizes can be mailed.
				You may opt out of the drawing by contacting the program coordinator at <a href='mailto:reblin@umich.edu'>reblin@umich.edu</a>.<P>
				All those invited to participate in the two follow-up surveys will receive a $10 gift card; with an additional $15 for completing the four week survey and an additional $25 for completing the five to six month survey.<P>



				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>CONTACT INFORMATION</DIV><P>
				<b>What if I have any questions about the study?</b><br>
				If you have questions about this research, the study questions, or this consent process, you can contact the Principal Investigator: Dr. Cheryl King at <a href='mailto:kingca@umich.edu'>kingca@umich.edu</a>
                or Dr. William Coryell at <a href='mailto:william-coryell@uiowa.edu'>william-coryell@uiowa.edu</a>.  If you experience a research-related injury, please contact: contact William Coryell, MD at <a href='mailto:william-coryell@uiowa.edu'>william-coryell@uiowa.edu</a>.  <BR><BR>

				If you have questions about the rights of research subjects, please contact the Human Subjects Office, 105 Hardin Library for the Health Sciences, 600 Newton Rd, The University of Iowa, Iowa City, IA  52242-1098, (319) 335-6564, or e-mail <a href='mailto:irb@uiowa.edu'>irb@uiowa.edu<a>.  
				To offer input about your experiences as a research subject or to speak to someone other than the research staff, call the Human Subjects Office at the number above.<P>

                Thank you very much for considering participating in this study! <P>

				</DIV>

				<DIV style='padding-top:5px;padding-bottom:20px'>If you wish to save or print a copy of this consent document for your own records, you can click <A href='../survey/question/CONSENT_IOWA.pdf' target='_blank'><B>here</B></A> to view a printable version.</I></DIV>

				<TABLE width='600px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>
				<TR height='25px'><TD style='padding-right:8px'><INPUT type='radio' name='Y' id='S'></TD><TD><SPAN onclick=""document.getElementById('S').click()"" style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=""this.style.backgroundColor='yellow'"" onmouseout=""this.style.backgroundColor='white'""><B>YES</B>, I have read the information above and consent to participate in this study.</SPAN></TD></TR>
				<TR height='25px'><TD style='padding-right:8px'><INPUT type='radio' name='Y' id='N'></TD><TD><SPAN onclick=""document.getElementById('N').click()"" style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=""this.style.backgroundColor='yellow'"" onmouseout=""this.style.backgroundColor='white'""><B>NO</B>, I do not wish to participate in this study and understand that there is no penalty for not participating.</SPAN></TD></TR>
				<TR height='5px'><TD></TD></TR>
				</TABLE>

				<CENTER style='padding-top:25px;padding-bottom:15px'><IMG src='../image/next.png' border='0' onclick=""if (document.getElementById('S').checked){window.location.href='screen.aspx?p=0';} else if(document.getElementById('N').checked){alert('Thanks for checking out the study. You may now close your browser window.');return false;} else {alert('Please select -YES- before proceeding; or -NO- to quit the survey.');return false;}"" style='cursor:hand;cursor:pointer'></CENTER>";
                break;
            case "m":
                consent_string = @"<CENTER><B style='font-size:16px'>Welcome to the Electronic Bridge to Mental Health (e-Bridge) for College Students Study</B></CENTER>

				<BR>&nbsp;<BR>

				Welcome to the <I>e</I>Bridge study of college student mental health! Our goal is to learn more about how to provide support to college students who are experiencing mental or emotional difficulties. If you participate in this study, you will be asked to complete a brief web survey, <I>which takes less than 5 minutes for most participants</I>. You will then receive automated feedback about your current emotional health as well as suggestions for types of support or services you may want to consider. Please read more information about the study below, and then indicate at the bottom of the screen whether you are willing to participate in this study.

				<BR>&nbsp;<BR>

				<DIV style='border:1px solid gray;padding:15px;width:588px;height:400px;overflow:auto'>

				<SPAN style='font-size:9px'>Study No: HUM00091681<BR>IRB: IRBMED<BR>Consent Approved On: 10/15/2014<BR>Project Approval Expires On: 10/14/2015</SPAN><P>

				<CENTER>
				<B>THE UNIVERSITY OF MICHIGAN<P>
				<B>CONSENT TO BE PART OF A RESEARCH STUDY</B><P>
				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px'>NAME OF STUDY AND RESEARCHERS</DIV><P>

				<B>Electronic Bridge to Mental Health (e-Bridge) for College Students</B><P>
				<B>Principal Investigator:</B> Cheryl A. King, Ph.D.<BR>
				<B>Co-Investigators:</B> Stephen Chermack, Ph.D., Daniel Eisenberg, Ph.D.<BR>
				Inbal Nahum-Shani, Ph.D., Kai Zheng, Ph.D.<BR>
				<B>Funding Provided by:</B> Department of Health and Human Services, National Institute of Mental Health<P>
				</CENTER>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>GENERAL INFORMATION</DIV><P>

				Researchers at the University of Michigan, University of Iowa, University of Nevada-Reno, and Stanford University are conducting research with college students to learn about <i>e</i>Bridge,
				a service to help link students with depression ot other emotional difficulties to services or resources that may be helpful.  We hope to learn about how to best implement <i>e</i>Bridge
				and to find out if it improves linkages to services or resources.<P>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>GENERAL INFORMATION</DIV><P>

				University of Michigan Students (18 years or older) are eligible to participate in the study.  We anticipate 909 students to participate at the University of Michigan, and 2305 across all sites.
				Your participation is voluntary--your decision of whether to participate will not affect your standing at this university in any way, and you will not be penalized in any way if you decide not to participate.
				You may discontinue participation at any time.<P>

				<b>What is involved if I participate in this study?</b><P>
				<b>Part 1:</b><br>
				If you agree to participate, we will ask you to complete an online survey that will take either take 2 to 5 minutes or 8 to 10 minutes, depending upon the number of questions incuded in your survey.
				All students will be asked questions about their age, race, ethnicity, thoughts, behaviors, and moods.  All students will be asked about their use of mental health services.  
				Some students will be asked additional questions about their emotions and behaviors, views about mental health services, and academic functioning.  The survey will require a total of 8 to 10 minutes for these students.
				We anticipate that approximately  5 to 10 percent of students will be presented these additional questions.  These students will be identified based on their responses to the first set of questions.
				They are students for whom we anticipate that the <i>e</i>Bridge intervention may be helpful.<P>

				<b>Part 2:</b><br>
				All students who complete the longer online survey will have an opportunity to view personalized feedback regarding their survey responses.  
				Approximately one-half of these students will be randomly selected to participate in the <i>e</i>Bridge intervention,
				which will present them with the option of exchanging online messages with a professional counselor about their personalized feedback , their concerns and/or the availability of resources in the campus community.
				For the randomization each student has approximately a 50-50 chance of of being assigned to the <i>e</i>Bridge group or the personalized feedback and information services group only.<P>

				We may obtain records from your university's counseling center or mental health clinic to determine if you have utilized mental health services during your participation in the study.
				We may also obtain your academic records (e.g. enrollment, grade point average) to assess your academic functioning during the study.  These records will be used for research purposes only and will be kept confidential by the research team.<P>

				<b>Personalized Feedback Group:</b><br>
				You will receive personalized feedback regarding your survey responses that will include information about local services available.<P>
				We will contact you four weeks later and approximately five to six months later for online follow-up surveys (lasting 6-10 minutes) about how your are doing and any services you have obtained.<P>
				<b>Online Message group:</b><br>
				You will receive personalized feedback regarding your survey responses that will include information about local services available and you will have the option to exchange online messages with professional counselor.<P>
				The counselor does not provide treatment and is unable to respond rapidly to online messages requiring immediate assistance for risk or harm to self or others.<P>
				We will contact you again four weeks later and approximately five to six months later for online follow-up surveys (lasting 6-10) about how you are doing and any services you have obtained.<P>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>FINANCIAL INFORMATION</DIV><P>
				<b>Will I be paid or given anything for taking part in the study?</b><br>
				Regardless of whether or not you participate, you wil be automatically entered into a random drawing for gift cards (ten $100 gift cards at each participating university).
				The drawing will be conducted at the University of Michigan on November 14, 2014.  Winners will be notified by email within three business days and asked to provide their names and mailing address so prizes can be mailed.
				You may opt out of the drawing by contacting the researchers by email.<P>
				If you participate in Part 2 of the study, you will receive a $10 gift card for completion of the intial survey.  You will also receive an additional $15 gift card for completing the four week survey
				and an additional $25 gift card for completing the five to six month survey.<P>

				<b>I have to pay anything to participate in the study?</b><br>
				There are no costs to you for participating in this study.<P>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>INFORMATION ABOUT RISKS AND BENEFITS</DIV><P>
				<b>What are the risks associated with my participation?</b><br>
				Some of the questions will ask you about sensitive or personal information such as your emotional health. 
				These questions might make you feel uncomfortable or anxious. At the end of the survey you will receive a list of resources on campus that can provide help and support if ever needed. 
				If responding to any questions makes you feel distressed, we urge you to call any of the resources listed. 
				There is also risk that information about you could be discovered by those who are not part of the research team. <P>

				<b>What are the benefits from my participation in the this research?</b><br>
				You may not receive any personal or direct benefits from being in this study.  We expect students may learn important information about available services. 
				Some students may also link to needed services as a result of study participation. 
				We expect this research to be used to gain an understanding of how to best provide links and resources for college students.<P>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>CONFIDENTIALITY OF SUBJECT RECORDS</DIV><P>

				<b>How will my privacy and confidentiality be protected?</b><br>
				We will do everything we can to protect your privacy. We will obtain a Certificate of Confidentiality from the National Institute of Health for this study. 
				This protects us from having to release information that could be used to identify you.  
				It allows us to refuse to disclose such information in any civil, criminal, administrative, legislative, or other proceeding, whether at the federal, state, or local level. 
				It does not however, prevent you from choosing to disclose information that we obtain in this study to physicians or others.<P>

				We will maintain a secure data environment using Secure Sockets Layer (SSL) encryption technology. 
				Although students are providing personal information transferred through a study website, we will never link this information to any of the study data (from the surveys and from the correspondence via the website). 
				We will only use identifying information (name  and email address) to recruit students for participation, 
				contact students who win gift cards in the random drawing, to obtain records (see additional information below), 
				or to contact you for optional participation in future phases of this project.  
				We will keep your contact information on file after the six month assessment in order to keep you updated on the progress of the study and to contact you for possible participation in future phases of the study. 
				Your contact information will be stored in a password protected data file, which will only be available to the research staff. <P>

				The data from this study, without any identifiable information, will be retained in a secure repository for future research purposes. 
				Records will be kept confidential to the extent provided by federal, state, and local law. 
				However, the Institutional Review Board, the sponsor of the study, or university and government officials responsible for monitoring this study may inspect these records.<P>

				Should you accidentally leave a session open on a computer that may be viewed by others, the computer will automatically log-off. 
				After 10 minutes of idle time, you will receive a notification that the session will be logged off in 1 minute unless you click to continue the session.<P>
					
				Agreeing to participate in this study gives the researchers your permission to obtain, use, and share information about you for this study, and is required in order for you to take part in the study.  
				Information about you may be obtained from any hospital, doctor, and other health care provider involved in your care, including:
				<ul>
					<li>Mental health care records (except psychotherapy notes not kept in your medical records)</li>
					<li>All records relating to your condition, the treatment you have received, and your response to the treatment</li>
				</ul><p>

				There are many reasons why information about you may be used or seen by the researchers or others during or after this study.  Examples include:
				<ul>
					<li>The researchers may need the information to make sure you can take part in the study</li>
					<li>The researchers may need the information to check your test results or look for side effects.  University, Food and Drug Administration (FDA), and/or other government officials may need the information to make sure that the study is done in a safe and proper manner. </li>
				</ul><p>

				<DIV style='width:100%;background-color:#ECECEC;border:1px solid black;padding:3px;text-align:center'>CONTACT INFORMATION</DIV><P>
				<b>What if I have any questions about the study?</b><br>
				If you have questions about this research, the study questions, or this consent process, you can contact the Principal Investigator: Dr. Cheryl King at <a href='mailto:kingca@umich.edu'>kingca@umich.edu</a><P>
				University of Michigan Medical School Institutional Review Board (IRBMED) 2800 Plymouth Road, Building 520, Room 3214, Ann Arbor, MI 48109-2800, 734-763-4768<P>
				If you are concerned about a possible violation of your privacy, contact the University of Michigan Health System Privacy Officer at 1-888-296-2481 <P>
				<i>When you call or write about a concern, please provide as much information as possible, including the name of the researcher, the IRBMED number (HUM00091681), and details about the problem.  
				This will help University officials to look into your concern.  When reporting a concern, you do not have to give your name unless you want to.</i><p>

				</DIV>

				<DIV style='padding-top:5px;padding-bottom:20px'>If you wish to save or print a copy of this consent document for your own records, you can click <A href='../survey/question/CONSENT_FORM.pdf' target='_blank'><B>here</B></A> to view a printable version.</I></DIV>

				<TABLE width='600px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>
				<TR height='25px'><TD style='padding-right:8px'><INPUT type='radio' name='Y' id='S'></TD><TD><SPAN onclick=""document.getElementById('S').click()"" style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=""this.style.backgroundColor='yellow'"" onmouseout=""this.style.backgroundColor='white'""><B>YES</B>, I have read the information above and consent to participate in this study.</SPAN></TD></TR>
				<TR height='25px'><TD style='padding-right:8px'><INPUT type='radio' name='Y' id='N'></TD><TD><SPAN onclick=""document.getElementById('N').click()"" style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=""this.style.backgroundColor='yellow'"" onmouseout=""this.style.backgroundColor='white'""><B>NO</B>, I do not wish to participate in this study and understand that there is no penalty for not participating.</SPAN></TD></TR>
				<TR height='5px'><TD></TD></TR>
				</TABLE>

				<CENTER style='padding-top:25px;padding-bottom:15px'><IMG src='../image/next.png' border='0' onclick=""if (document.getElementById('S').checked){window.location.href='screen.aspx?p=0';} else if(document.getElementById('N').checked){alert('Thanks for checking out the study. You may now close your browser window.');return false;} else {alert('Please select -YES- before proceeding; or -NO- to quit the survey.');return false;}"" style='cursor:hand;cursor:pointer'></CENTER>";
                break;
            default:
                throw new System.ArgumentException("School code did not match");
        }
        return consent_string;
    }
    //page footer data with contact info

    public static string EmergencyData(string schoolcode)
    {
        string emergencydata="";
        switch (schoolcode)
        {
            case "i":
                emergencydata= @"<TR><TH align='left' colspan='3'><U>Emergency Resources (for 24/7 help):</U></TH></TR>
                                 <TR>
                                 <TD valign='top' align='left' style = 'width:40%'>
                                 <br>
                                 <li>
                                 dial “911”
                                 </li>
                                 <br>
                                 <li>
                                 Johnson County Crisis Center
                                 <br>&nbsp&nbsp 319-351-0140
                                 </li>
                                 </TD>
                                 <TD valign='top' align='left' style = 'width:15%'></TD>
                                 <TD valign='top' align='left' style = 'width:40%'>
                                 <br>University of Iowa Hospitals and Clinics 
                                 <br>Emergency Treatment Center
                                 <br> 200 Hawkins Drive Iowa City, IA 52242
                                 <br> First floor, by Elevator E 
                                 <br> 319-356-2233
                                 </TD>
                                 </TR>
                                 </TR>";
                        break;
        }
        return emergencydata;
    }    

    public static string ContactData(string participant_id)
    {   

        string school_code = participant_id.Substring(participant_id.Length - 1);
        string contact_data;
        switch (school_code)
        {
            case "s":
                contact_data = @"<TABLE cellpadding='0px' cellspacing='0px' style='line-height:15px;font-size:14px;color:#FFFFFF;padding-top:20px'>
                                 <THEAD>
                                    <TR>
                                        <TD>
                                        <U>Resources at Stanford University</U>
                                        </TD>
                                    </TR>
                                    <TR>
                                        <TD>
                                        &nbsp  
                                        </TD>
                                    </TR>
                                 </THEAD>
                                 <TBODY>
                                 <TR>
                                 <TD valign='top' align='left' rowspan='2'>
                                 <P><b>For emergencies, call 911 (or 9-911 from a campus phone)</b>
                                 <P><a href= '/ebridge/redirect.aspx?action=resources&url=s1' style='color:#FFFFFF' target='_blank'>University Police (Stanford University Department of Public Safety)</a>, 24-hour
                                 <P><a href= '/ebridge/redirect.aspx?action=resources&url=s5' style='color:#FFFFFF' target='_blank'>Stanford University Confidential Sexual Assault Counselors</a>, 24-hour and confidential
                                 <P>650-725-9955
                                 <P><a href='/ebridge/redirect.aspx?action=resources&url=s2' style='color:#FFFFFF' target='_blank'>CAPS (Counseling and Psychological Services)</a>, <br/> 24-hour and confidential
                                 <P>650-723-3785
                                <P><a href='/ebridge/redirect.aspx?action=resources&url=s3' style='color:#FFFFFF' target='_blank'>Deans on Call</a>, 24-hour
                                <P><a href='/ebridge/redirect.aspx?action=resources&url=s4' style='color:#FFFFFF' target='_blank'>Undergraduate Residence Dean</a>
                                <P>650-725-2800 <u>(M-F, 8 am-5 pm)</u>
                                <P>650-723-2300 + ask to speak to the Undergraduate Residence Dean <u>(after 5 pm and weekends)</u>
                                <P>Graduate Life Dean
                                <P>650-736-7078 <u>(M-F, 8 am-5 pm)</u>
                                <P>650-723-8222 + enter Pager ID 25085 <br/><u>(after 5 pm and weekends)</u>
                                <P>Confidential Assistance for Postdocs, <br/> 24-hour and confidential
                                <P>855-666-0519
                                <P>Bridge Peer Counseling Center, <br/> 24-hour and confidential
                                <P>650-723-3392
                                 </TD>

                                <TD style='padding-left:5px; border-bottom: 1px dotted #000'>

                                <P>San Francisco Suicide Prevention, <br/> 24-hour and confidential
                                <P>415-781-0500
                                <P>YWCA Rape Crisis Center, Silicon Valley, <br/> 24-hour and confidential
                                <P>650-493-7273
                                <P>YWCA Domestic Violence Hotline, Silicon Valley, <br/> 24 hour and confidential
                                <P>1-800-572-2782
                                <P>National Dating Abuse Helpline, <br/> 24-hour and confidential
                                <P>1-866-331-9474
                                <P>National Domestic Violence Hotline, <br/> 24-hour and confidential
                                <P>1-800-799-7233
                                <BR/>&nbsp;

                                 </TD>

                                 </TR>

                                 <TR>
                                 <TD style='padding-left:5px'>
                                 <BR/>
                                 <P><a href='/ebridge/redirect.aspx?action=resources&url=s7' style='color:#FFFFFF' target='_blank'> Ronald C. Albucher, M.D.</a>
                                 <P>Director, Counseling and Psychological Services

                                 <P>Vaden Health Center
                                 <BR/>Stanford University
                                 <BR/>866 Campus Drive
                                 <BR/>Stanford, CA 94305
                                 
                                 <P>Tel: 650-723-3785 
                                 <BR/>Direct Line: 650-725-1357 
                                 <BR/>Fax: 650-725-2887
                                 <BR/>Email: albucher@stanford.edu
                                 <BR/>WEB SITE: <a href='/ebridge/redirect.aspx?action=resources&url=s7' style='color:#FFFFFF' target='_blank'>http://caps.stanford.edu</a>

                                 <P> For urgent situations, you can phone CAPS 24 hours a day seven days a week at (650) 723-3785
                                 </TD>
                                 </TR>
                                 </TBODY></TABLE>";
                break;
            case "n":
                contact_data = @"<TABLE cellpadding='0px' cellspacing='0px' style='line-height:15px;font-size:11px;color:#FFFFFF;padding-top:20px'>
                                 <TR>
                                 <TD valign='top' align='left'><U>Resources at the University of Nevada, Reno</U><br>
                                 <P><b>IF IT&#39;S AN EMERGENCY, CALL 911. </b>
                                 <P>IF IT&#39;S URGENT and during off hours, contact the Crisis Call (24-hour help available via phone or text)  
                                 <P>Phone 775-784-8090  or Text “listen” to 839863<br>
                                 <P><u>During Business Hours, contact:</u>
                                 <P>Counseling Services (CS): FREE psychological counseling for students enrolled in 6+ credits (otherwise, can pay $35 counseling fee per semester at MyNevada). 
                                 <P>Phone: 775-784-4648
                                 <P><a href = '/ebridge/redirect.aspx?action=resources&url=n1' style='color:#FFFFFF' target='_blank'>http://www.unr.edu/counseling</a><br>
                                 <P>For psychiatric medication management, contact Student Health Services (SHC)
                                 <P>775-784-6598
                                 <P><a href = '/ebridge/redirect.aspx?action=resources&url=n2' style='color:#FFFFFF' target='_blank'>http://www.unr.edu/shc/</a>
                                 </TD></TR></TABLE>";

                break;
            case "i":
                contact_data = @"<TABLE cellpadding='0px' cellspacing='0px' style='line-height:15px;font-size:14px;color:#FFFFFF;padding-top:20px'>
                                 <TR><TH align='left' colspan='3'><U>Resources at the University of Iowa</U></TH></TR>
                                 <TR>
                                 <TD valign='top' align='left' style = 'width:40%'>
                                 <br>University of Iowa Student Health
                                 <br>4189 Westlawn South
                                 <br>Iowa City, Iowa 52242
                                 <br>319-335-8370
                                 <br><a href = '/ebridge/redirect.aspx?action=resources&url=i1' style='color:#FFFFFF' target='_blank'>http://studenthealth.uiowa.edu/</a><br><br>
                                 </TD>
                                 <TD valign='top' align='left' style = 'width:15%'></TD>
                                 <TD valign='top' align='left' style = 'width:40%'>
                                 <br>University of Iowa Counseling Service
                                 <br>3223 Westlawn South
                                 <br>Iowa City, Iowa 52242
                                 <br>319-335-7294
                                 <br><a href = '/ebridge/redirect.aspx?action=resources&url=i2' style='color:#FFFFFF' target='_blank'>http://counseling.studentlife.uiowa.edu/</a><br><br>
                                 </TD>
                                 </TR>
                                 </TR>
                                 _EMERGENCY_RESOURCES_
                                 <TR>
                                 <TD>
                                 &nbsp        
                                 </TD>
                                 </TR></TABLE>";
                break;
            case "m":
                contact_data = @"<TABLE cellpadding='5px' cellspacing='0px' style='line-height:15px;font-size:14px;color:#FFFFFF;padding-top:20px'>
                                 <TR><TH align='left' colspan='2'><u>Resources at the University of Michigan</u><P><P></TH></TR>
                                 <TR>
                                 <TD valign='top' align='left' width='%50'>
                                 
                                 <B>Psychiatric Emergency Services (for immediate help):</B><br>
                                    1500 East Medical Center Drive, Floor B1, Room B1B205<br>
                                    (734) 996-4747
                                 <P><B>Counseling and Psychological Services</B><BR>3100 Michigan Union<BR>(734) 764-8312<BR><A href='/ebridge/redirect.aspx?action=resources&url=m1' style='color:#FFFFFF' target='_blank'>http://www.umich.edu/~caps</A>
                                 <P><B>Psychological Clinic</B><BR>530 Church Street, East Hall<BR>(734) 764-3471<BR><A href='/ebridge/redirect.aspx?action=resources&url=m1' style='color:#FFFFFF' target='_blank'>http://www.umich.edu/~psychcln/</A></TD>
                                 <TD valign='top' align='left' width='%50'><B>University Health Services</B><BR>207 Fletcher Street<BR>(734) 764-8320<BR><A href='/ebridge/redirect.aspx?action=resources&url=m1' style='color:#FFFFFF' target='_blank'>http://www.uhs.umich.edu</A>
                                 <P><B>National hotline for mental health emergencies (in case you are currently outside the Ann Arbor area):<BR>1-800-273-TALK</B></TD></TR></TABLE>";
                break;
            default:
                throw new System.ArgumentException("School code did not match");
        }
        return contact_data;
    }

    public static string ContactDataOther(string participant_id)
    {
        string school_code = participant_id.Substring(participant_id.Length - 1);
        string contact_data;
        switch (school_code)
        {
            case "s":
                contact_data = @"<TABLE cellpadding='0px' cellspacing='0px' style='line-height:15px;font-size:11px;color:#FFFFFF;padding-top:20px'>
                                 <TR><TD valign='top' align='left'><U>Resources at Stanford University</U><br>
                                 <P><b>For emergencies, call 911 (or 9-911 from a campus phone)</b>
                                 <P><a href= 'ebridge/redirect.aspx?action=resources&url=s1' style='color:#FFFFFF' target='_blank'>University Police (Stanford University Department of Public Safety)</a>, 24-hour
                                 <P><a href= '/ebridge/redirect.aspx?action=resources&url=s5' style='color:#FFFFFF' target='_blank'>Stanford University Confidential Sexual Assault Counselors</a>, <u>24-hour and confidential</u>
                                 <P>650-725-9955
                                 <P><a href='/ebridge/redirect.aspx?action=resources&url=s2' style='color:#FFFFFF' target='_blank'>CAPS (Counseling and Psychological Services)</a>, <u>24-hour and confidential</u>
                                 <P>650-723-3785
                                <P><a href='/ebridge/redirect.aspx?action=resources&url=s3' style='color:#FFFFFF' target='_blank'>Deans on Call</a>, <u>24-hour</u>
                                <P><a href='/ebridge/redirect.aspx?action=resources&url=s4' style='color:#FFFFFF' target='_blank'>Undergraduate Residence Dean</a>
                                <P>650-725-2800 <u>(M-F, 8 am-5 pm)</u>
                                <P>650-723-2300 + ask to speak to the Undergraduate Residence Dean <u>(after 5 pm and weekends)</u>
                                <P>Graduate Life Dean
                                <P>650-736-7078 <u>(M-F, 8 am-5 pm)</u>
                                <P>650-723-8222 + enter Pager ID 25085 <u>(after 5 pm and weekends)</u>
                                <P>Confidential Assistance for Postdocs, 24-hour and confidential
                                <P>855-666-0519
                                <P>Bridge Peer Counseling Center, 24-hour and confidential
                                <P>650-723-3392
                                <P>San Francisco Suicide Prevention, 24-hour and confidential
                                <P>415-781-0500
                                <P>YWCA Rape Crisis Center, Silicon Valley, 24-hour and confidential
                                <P>650-493-7273
                                <P>YWCA Domestic Violence Hotline, Silicon Valley, 24 hour and confidential
                                <P>1-800-572-2782
                                <P>National Dating Abuse Helpline, 24-hour and confidential
                                <P>1-866-331-9474
                                <P>National Domestic Violence Hotline, 24-hour and confidential
                                <P>1-800-799-7233
                                <P> Ronald C. Albucher, M.D.
                                 <P>
                                 <P>Director, Counseling and Psychological Services
                                 <P>Vaden Health Center
                                 <P>Stanford University
                                 <P>866 Campus Drive
                                 <P>Stanford, CA 94305
                                 <P>
                                 <P>Tel: 650-723-3785 
                                 <P>Direct Line: 650-725-1357 
                                 <P>Fax: 650-725-2887
                                 <P>Email: albucher@stanford.edu
                                 <P>WEB SITE: <a href='/ebridge/redirect.aspx?action=resources&url=s7' style='color:#FFFFFF' target='_blank'>http://caps.stanford.edu</a>
                                 <P>
                                 <P> For urgent situations, you can phone CAPS 24 hours a day seven days a week at (650) 723-3785
                                 </TD></TR></TABLE>";
                break;
            case "n":
                contact_data = @"<TABLE cellpadding='0px' cellspacing='0px' style='line-height:15px;font-size:11px;color:#FFFFFF;padding-top:20px'>
                                 <TR>
                                 <TD valign='top' align='left'><U>Resources at the University of Nevada, Reno</U><br>
                                 <P><b>IF IT&#39;S AN EMERGENCY, CALL 911. </b>
                                 <P>IF IT&#39;S URGENT and during off hours, contact the Crisis Call (24-hour help available via phone or text)  
                                 <P>Phone 775-784-8090  or Text “listen” to 839863<br>
                                 <P><u>During Business Hours, contact:</u>
                                 <P>Counseling Services (CS): FREE psychological counseling for students enrolled in 6+ credits (otherwise, can pay $35 counseling fee per semester at MyNevada). 
                                 <P>Phone: 775-784-4648
                                 <P><a href = '/redirect.aspx?action=resources&url=n1' style='color:#FFFFFF' target='_blank'>http://www.unr.edu/counseling</a><br>
                                 <P>For psychiatric medication management, contact Student Health Services (SHC)
                                 <P>775-784-6598
                                 <P><a href = '/redirect.aspx?action=resources&url=n2' style='color:#FFFFFF' target='_blank'>http://www.unr.edu/shc/</a>
                                 </TD></TR></TABLE>";

                break;
            case "i":
                contact_data = @"<TABLE cellpadding='0px' cellspacing='0px' style='line-height:15px;font-size:14px;color:#FFFFFF;padding-top:20px'>
                                 <TR><TD valign='top' align='left'><U>Resources at the University of Iowa</U>
                                 <br>
                                 <br>University of Iowa Student Health
                                 <br>4189 Westlawn South
                                 <br>Iowa City, Iowa 52242
                                 <br>319-335-8370
                                 <br><a href = '/redirect.aspx?action=resources&url=i1' style='color:#FFFFFF' target='_blank'>http://studenthealth.uiowa.edu/</a><br><br>
                                 <P>University of Iowa Counseling Service
                                 <br>3223 Westlawn South
                                 <br>Iowa City, Iowa 52242
                                 <br>319-335-7294
                                 <br><a href = '/redirect.aspx?action=resources&url=i2' style='color:#FFFFFF' target='_blank'>http://counseling.studentlife.uiowa.edu/</a><br><br>
                                 </TD></TR>
                                 <TR><TD valign='top' align='left'><u>Emergency Resources (for 24/7 help):</u>
                                 <p>dial “911”</p>
                                 <p>Johnson County Crisis Center 
                                 <br>319-351-0140 
                                 </p>
                                 <p>University of Iowa Hospitals and Clinics
                                 <br>Emergency Treatment Center 
                                 <br>200 Hawkins Drive Iowa City, IA 52242 
                                 <br>First floor, by Elevator E
                                 <br>319-356-2233
                                 </p>
                                 </TABLE>";
                break;
            case "m":
                contact_data = @"<TABLE cellpadding='5px' cellspacing='0px' style='line-height:15px;font-size:14px;color:#FFFFFF;padding-top:20px'>
                                 <TR><TH align='left' colspan='2'><u>Resources at the University of Michigan</u><P><P></TH></TR>
                                 <TR>
                                 <TD valign='top' align='left' width='%50'>
                                 
                                 <B>Psychiatric Emergency Services (for immediate help):</B><br>
                                    1500 East Medical Center Drive, Floor B1, Room B1B205<br>
                                    (734) 996-4747
                                 <P><B>Counseling and Psychological Services</B><BR>3100 Michigan Union<BR>(734) 764-8312<BR><A href='/redirect.aspx?action=resources&url=m1' style='color:#FFFFFF' target='_blank'>http://www.umich.edu/~caps</A>
                                 <P><B>Psychological Clinic</B><BR>530 Church Street, East Hall<BR>(734) 764-3471<BR><A href='/redirect.aspx?action=resources&url=m1' style='color:#FFFFFF' target='_blank'>http://www.umich.edu/~psychcln/</A>
                                 <P><B>University Health Services</B><BR>207 Fletcher Street<BR>(734) 764-8320<BR><A href='/redirect.aspx?action=resources&url=m1' style='color:#FFFFFF' target='_blank'>http://www.uhs.umich.edu</A>
                                 <P><B>National hotline for mental health emergencies (in case you are currently outside the Ann Arbor area):<BR>1-800-273-TALK</B></TD></TR></TABLE>";
                break;
            default:
                throw new System.ArgumentException("School code did not match");
        }
        return contact_data;

    }
    public static string WarningMessage(string participant_id)
    {
        string school_code = participant_id.Substring(participant_id.Length - 1);
        string warning_data;
        switch (school_code)
        {
            case "s":
                warning_data = @"<TABLE id='W' cellpadding='0' cellspacing='0' style='display:none;font-family:arial;font-size:12px;padding-right:10px'>
                                 <TR height='20px'>
                                 <TD style='border:1px solid red;padding:8px'><B>Precautionary Note:</B> If you have serious thoughts of harming yourself, please dial '911' or call Stanford Counseling and Psychological Services at 650-723-3785</TD>
                                 </TR>
                                 <TR height='20px'><TD></TD></TR>
                                 </TABLE>";
                break;
            case "n":
                warning_data = @"<TABLE id='W' cellpadding='0' cellspacing='0' style='display:none;font-family:arial;font-size:12px;padding-right:10px'>
                                 <TR height='20px'>
                                 <TD style='border:1px solid red;padding:8px'><B>Precautionary Note:</B> If you have serious thoughts of harming yourself, please dial '911' or contact Crisis Call at 775-784-8090</TD>
                                 </TR>
                                 <TR height='20px'><TD></TD></TR>
                                 </TABLE>";
                break;
            case "i":
                warning_data = @"<TABLE id='W' cellpadding='0' cellspacing='0' style='display:none;font-family:arial;font-size:12px;padding-right:10px'>
                                 <TR height='20px'>
                                 <TD style='border:1px solid red;padding:8px'><B>Precautionary Note:</B> If you have serious thoughts of harming yourself, please dial “911,” or call the Johnson County Crisis Center at 319-351-0140.  You may also go to the University of Iowa Hospitals and Clinics Emergency Treatment Center (the emergency room) at 200 Hawkins Drive Iowa City, IA 52242, first floor, by Elevator E or call at 319-356-2233.</TD>
                                 </TR>
                                 <TR height='20px'><TD></TD></TR>
                                 </TABLE>";
                break;
            case "m":
                warning_data = @"<TABLE id='W' cellpadding='0' cellspacing='0' style='display:none;font-family:arial;font-size:12px;padding-right:10px'>
                                 <TR height='20px'>
                                 <TD style='border:1px solid red;padding:8px'><B>Precautionary Note:</B> If you have serious thoughts of harming yourself, please dial '911', call Psychiatry Emergency Services at 734-996-4747, or go immediately to Psychiatric Emergency Services at the University of Michigan Hospital: University Hospital, 1500 East Medical Center Drive, Floor B1, Room B1B205.</TD>
                                 </TR>
                                 <TR height='20px'><TD></TD></TR>
                                 </TABLE>";
                break;
            default:
                throw new System.ArgumentException("School code not contained in Participant ID");
        }
        return warning_data;   
    }
    public static Dictionary<string, string> ConsentContent(string participant_id)
    {
        
        Dictionary<string, string> content_dict = new Dictionary<string,string>();

        string school_code = participant_id.Substring(participant_id.Length - 1);
        switch (school_code)
        {
            case "s":
                content_dict["school_title"] = "Stanford University";
                content_dict["irb_info"] = "STANFORD_IRB_INFO";
                content_dict["privacy_info"] = "STANFORD_PRIVACY_INFO";
                content_dict["secondary_contact"] = "STANFORD_SECONDARY_CONTACT";
                content_dict["drawing_info"] = "STANFORD_DRAWING_INFO";
                break;
            case "n":
                content_dict["school_title"] = "the University of Nevada, Reno";
                content_dict["irb_info"] = "UNR_IRB_INFO";
                content_dict["privacy_info"] = "UNR_PRIVACY_INFO";
                content_dict["secondary_contact"] = "UNR_SECONDARY_CONTACT";
                content_dict["drawing_info"] = "UNR_DRAWING_INFO";
                break;
            case "i":
                content_dict["school_title"] = "the University of Iowa";
                content_dict["irb_info"] = "IOWA_IRB_INFO";
                content_dict["privacy_info"] = "IOWA_PRIVACY_INFO";
                content_dict["secondary_contact"] = "IOWA_SECONDARY_CONTACT";
                content_dict["drawing_info"] = "IOWA_DRAWING_INFO";
                break;
            case "m":
                content_dict["school_title"] = "the University of Michigan";
                content_dict["irb_info"] = "University of Michigan Medical School Institutional Review Board (IRBMED): 2800 Plymouth Road, Building 200, Room 2086, Ann Arbor, MI 48109-2800, (734) 763-4768.";
                content_dict["privacy_info"] = "the University of Michigan Health System Privacy Officer at 1-888-296-2481.";
                content_dict["secondary_contact"] = "Dr. Daniel Eisenberg at <A HREF='daneis@umich.edu'>daneis@umich.edu</A> or the Study Coordinator: Anne Kramer, at ack@umich.edu or (734) 764-3168.";
                content_dict["drawing_info"] = "S_DRAWING_INFO";
                break;
            default:
                throw new System.ArgumentException("School code not contained in Participant ID");
        }
        return content_dict;
        
    }


    public static string ChatTimes(string _style, string participant_id)
    {

        Dictionary<string, string> content_dict = new Dictionary<string, string>();

        string school_code = participant_id.Substring(participant_id.Length - 1);
        string chat_times="";
        string QueryString = "select hours from site s, (select max(date_time) as max_date from site where id='{0}') as r where s.id='{0}' and s.date_time = r.max_date;";
        string _hours = Db.GetRecord(string.Format(QueryString, school_code))[0];
        string[] _days = _hours.Split('|');

        for (int i = 0; i < _days.Length; i++)
        {
            chat_times += "<TR><TD {0}>" + _days[i] + "</TD></TR>";
        }
        return String.Format(chat_times, _style);

    }

    public static int RandomizeGroup(string SiteCode, string ParticipantId)
    {

        // old version, after tidying up

        Random _random_first = new Random();

        // retrieve gender and year and school
        string[] _profile = Db.GetRecord("SELECT gender, year FROM participant WHERE id = '" + ParticipantId + "'");
        string _gender = _profile[0]; string _year = _profile[1]; string _school = SiteCode;

        // count how many students have been assigned into the intervention group of each genera/year combination
        string QueryString = "SELECT COUNT(*) FROM participant AS p, status AS s WHERE p.id = s.id "
            + "AND s.status_code = 'INTERVENTION' AND s.status_value = '{0}' AND p.gender = '{1}' AND p.year = '{2}' "
            + "AND RIGHT(p.id, 1) =  '{3}'";
        // _intervention1: students in same gender and year as the participant already in the control group, where status_value is 0
        int _intervention1 = Db.GetCount(string.Format(QueryString, new string[4] { "0", _gender, _year, _school }));

        // _intervention2: students in same gender and year as the participant already in the intervention group, where status_value is 1
        int _intervention2 = Db.GetCount(string.Format(QueryString, new string[4] { "1", _gender, _year, _school }));

        // for use inside with counselor balancing
        QueryString = "SELECT COUNT(*) FROM status WHERE status_code = 'COUNSELOR' AND status_value = '{0}' AND RIGHT(id, 1) = '{1}'";

        // if no one from that gender/year group has been randomized into the intervention group yet
        if (_intervention1 == 0 && _intervention2 == 0)
        {
            // use seed number to randomize the student
            string _group_first = _random_first.Next(0, 2).ToString();

            //TODO~yffu would like to change this to after the counselor assignment, so that it can say "CONTROL as well", but may break some things. 
            Utility.UpdateStatus(ParticipantId, "INTERVENTION", _group_first);

            // counselor assignment - intervention group and counseler assignment
            if (_group_first == "1")
            {
                int _counselor1 = Db.GetCount(string.Format(QueryString, new string[2] { "C001", ParticipantId }));
                int _counselor2 = Db.GetCount(string.Format(QueryString, new string[2] { "C002", ParticipantId }));
                // if there are more students assigned to counselor 1 than 3/5 of counselor 2.  
                // does that mean counselor 1 takes more students? seems like it's not really random.
                if (_counselor1 >= 3 * _counselor2 / 5) Utility.UpdateStatus(ParticipantId, "COUNSELOR", "C002");
                else Utility.UpdateStatus(ParticipantId, "COUNSELOR", "C001");
            }
            else
            {
                // do nothing. is this balanced?
            }
        }
        else
        {
            // if more or equal number in the control compared to the intervention
            if (_intervention1 >= _intervention2)
            {
                //put the person in the intervention group
                Utility.UpdateStatus(ParticipantId, "INTERVENTION", "1");

                // counselor assignment
                int _counselor1 = Db.GetCount(string.Format(QueryString, new string[2] { "C001", ParticipantId }));
                int _counselor2 = Db.GetCount(string.Format(QueryString, new string[2] { "C002", ParticipantId }));

                if (_counselor1 >= 3 * _counselor2 / 5) Utility.UpdateStatus(ParticipantId, "COUNSELOR", "C002");
                else Utility.UpdateStatus(ParticipantId, "COUNSELOR", "C001");
            }
            else
            {
                // put the person in the control group
                Utility.UpdateStatus(ParticipantId, "INTERVENTION", "0");
            }
        }
        return 0;
    }

    public static int RandomizeGroup1(string SiteCode, string ParticipantId)
    {
        Random _random_first = new Random();
        int _group=0;

        // if the person has had multiple suicide attemps - PHQ_SUI2[9] > 1, then immediately balance among control and intervention within school
        // regardless of the year/gender stratification. - numbers probably too small to do that effectively
        if (MultipleSuicide(ParticipantId))
        {
            string SuicideQuery =
            @"select count(*) from SCREENING_RESPONSE sr,
            STATUS s where
            sr.question_code='PHQ2_SUI'
            and right(sr.PARTICIPANT_ID, 1) = '{1}'
            and convert(int, substring(sr.response, 19, 1)) > 1
            and sr.PARTICIPANT_ID= s.ID 
            and s.STATUS_CODE='INTERVENTION'
            and s.STATUS_VALUE = '{0}'";
            int _suicide_risk_1 = Db.GetCount(string.Format(SuicideQuery, new string[2]{"0", SiteCode}));
            int _suicide_risk_2 = Db.GetCount(string.Format(SuicideQuery, new string[2]{"1", SiteCode}));
            if (_suicide_risk_1 >= _suicide_risk_2)
            {
                Utility.UpdateStatus(ParticipantId, "INTERVENTION", "1");
                AssignCounselor(ParticipantId, SiteCode);
                _group=1;
            }
            else 
            {
                Utility.UpdateStatus(ParticipantId, "INTERVENTION", "0");
                _group = 0;
            }
            return _group;
        }

        // retrieve gender and year and school
        string[] _profile = Db.GetRecord("SELECT gender, year FROM participant WHERE id = '" + ParticipantId + "'");
        string _gender = _profile[0]; string _year = _profile[1]; string _school = SiteCode;

        // count how many students have been assigned into the intervention group of each genera/year combination
        string QueryString = "SELECT COUNT(*) FROM participant AS p, status AS s WHERE p.id = s.id "
            + "AND s.status_code = 'INTERVENTION' AND s.status_value = '{0}' AND p.gender = '{1}' AND p.year = '{2}' "
            + "AND RIGHT(p.id, 1) =  '{3}'";
        // _intervention1: students in same gender and year as the participant already in the control group, where status_value is 0
        int _intervention1 = Db.GetCount(string.Format(QueryString, new string[4] { "0", _gender, _year, _school }));

        // _intervention2: students in same gender and year as the participant already in the intervention group, where status_value is 1
        int _intervention2 = Db.GetCount(string.Format(QueryString, new string[4] { "1", _gender, _year, _school }));

        // if no one from that gender/year group has been randomized into the intervention group yet
        if (_intervention1 == 0 && _intervention2 == 0)
        {
            // use seed number to randomize the student
            string _group_first = _random_first.Next(0, 2).ToString();

            //TODO~yffu would like to change this to after the counselor assignment, so that it can say "CONTROL as well", but may break some things. 
            Utility.UpdateStatus(ParticipantId, "INTERVENTION", _group_first);

            // counselor assignment - intervention group and counseler assignment
            if (_group_first == "1")
            {
                // assigncounselor might not work, since it'll turn up empty array - fixed with condition in assigncounselor
                AssignCounselor(ParticipantId, _school);
                _group = 1;
            }
            else
            {
                _group = 0;
            }
        }
        else
        {
            // if more or equal number in the control compared to the intervention
            if (_intervention1 >= _intervention2)
            {
                //put the person in the intervention group
                Utility.UpdateStatus(ParticipantId, "INTERVENTION", "1");
                _group=1;

                // counselor assignment
                AssignCounselor(ParticipantId, _school);
            }
            else
            {
                // put the person in the control group
                Utility.UpdateStatus(ParticipantId, "INTERVENTION", "0");
                _group=0;
            }
        }
        return _group;
    }

    public static bool MultipleSuicide(string ParticipantId)
    {
        bool mult_suicide=false;
        string QueryString =
        @"select RESPONSE from SCREENING_RESPONSE where QUESTION_CODE='PHQ2_SUI'
        and PARTICIPANT_ID='{0}'";
        try
        {
            string Response = Db.GetRecord(string.Format(QueryString, ParticipantId))[0];
            if (Convert.ToInt16(Response.Split('|')[9]) > 1) mult_suicide = true;
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine("Index out of range for response");
        }
        catch (FormatException e)
        {
            Console.WriteLine("Input string is not a sequence of digits.");
        }
        return mult_suicide;
    }

    public static void AssignCounselor(string ParticipantId, string _school)
    {
        Random _random_first = new Random();
        string[] assigned_counselor;
        string[][] available_counselors;
        int _min_participants;

        // all counselors that have zero participants assigned;
        string QueryString=
        @"select c.id, c.email from counselor c where not exists
        (select distinct status_value from status s
        where right(id, 1) ='{0}' and status_code='COUNSELOR'
        and c.id=s.status_value) and c.site='{0}'";
        available_counselors = Db.GetRecords(string.Format(QueryString, _school));        
    
        if (available_counselors!= null && available_counselors.Length !=0)
        {
            assigned_counselor = available_counselors[_random_first.Next(0, available_counselors.Length)];
            Utility.UpdateStatus(ParticipantId, "COUNSELOR", assigned_counselor[0]);
            return;
        }

        string QueryString1 = @"select min(pnum) from (
        select status_value, count(*) as pnum from status 
        where status_code='COUNSELOR'
        and right(id, 1)='{0}' group by status_value) as pcount";
        // find the count of the mininum participant assigned to any counselor
        
        _min_participants = Db.GetCount(string.Format(QueryString1, _school));

        // find the available counselors
        string QueryString2 =
        @"select c.id, c.email from counselor as c,
                (select s.status_value
                from status s
                where status_code='COUNSELOR' and right(s.id, 1)='{0}'
                group by status_value having count(*)={1}) as available
                where available.status_value = c.id
                and c.site='{0}'";

        available_counselors = Db.GetRecords(string.Format(QueryString2, new string[2] { _school, _min_participants.ToString() }));
        int counselors_cnt = available_counselors.Length;

        if (counselors_cnt != 0)
        {
            // use seed number to randomize the counselor
            assigned_counselor = available_counselors[_random_first.Next(0, counselors_cnt)];
            Utility.UpdateStatus(ParticipantId, "COUNSELOR", assigned_counselor[0]);

            //Db.SendGmail();
            return;
        }
        /*
         * used to be like this
            int _counselor1 = Db.GetCount(string.Format(QueryString, new string[2] { "C001", ParticipantId }));
            int _counselor2 = Db.GetCount(string.Format(QueryString, new string[2] { "C002", ParticipantId }));

            if (_counselor1 >= 3 * _counselor2 / 5) Utility.UpdateStatus(ParticipantId, "COUNSELOR", "C002");
            else Utility.UpdateStatus(ParticipantId, "COUNSELOR", "C001");
         */
    
    }


    public static string FormatChatUrl(string Pid, string Cid) 
    {
        string ChatUrl = "<A href='../chat/?cid=1&dtype=p&pid={0}&c={1}' target='_blank' style='color:blue'>";
        string[] args = { Pid, Cid };
        ChatUrl = String.Format(ChatUrl, args);
        ChatUrl += "<SPAN id='counselors' style='color:blue;text-decoration:underline'><B>Chat with Counselor</B></SPAN>";
        ChatUrl += "</A>";
        return ChatUrl;
    }

    public static string GetChatUrl(string ParticipantId, string DesignatedCounselor)
    {

        // chat for students view
        // http://localhost:53422/chat/?p=1&p1=1&p2=G6tE9FJ9gi&p3=3

        // schedule for students view
        // http://localhost:53422/chat/schedule.aspx?p=C001

        Random random = new Random();

        string _site = ParticipantId[ParticipantId.Length - 1].ToString();
        string QueryString1 = "SELECT COUNT(*) FROM ONLINE_STATUS WHERE DESIGNATION='COUNSELOR' AND ID='{0}' AND SITE= '{1}'";
        string QueryString2 = "SELECT COUNT(*) from ONLINE_STATUS where DESIGNATION='COUNSELOR' AND SITE = '{0}'";

        if (Db.GetCount(string.Format(QueryString1, new string[2] { DesignatedCounselor, _site })) == 1)
        {
            //Utility.SendPHPMail(ParticipantEmail, "ebridgeteam@umich.edu", "The eBridge Team", "your appointment to chat with ebridge counselor", EmailBody);

            return FormatChatUrl(ParticipantId, DesignatedCounselor);
        }

        int CountCounselor = Db.GetCount(string.Format(QueryString2, _site));

        if (CountCounselor > 0)
        {
            QueryString2 = "select ID from ONLINE_STATUS where ID LIKE '%C[0-9]%' AND SITE = '{0}'";
            string[][] CounselorsOnline = Db.GetRecords(string.Format(QueryString2, _site));

            int RandomCounselor = random.Next(0, CountCounselor);

            return FormatChatUrl(ParticipantId, CounselorsOnline[RandomCounselor][0]);
        }
        else
        {
            return "<SPAN id='counselors' style='color:red'><B>No Counselors Online</B></SPAN>";
        }

    }

    public static Dictionary<string, string> ListQueryStrings(string _school)
    {   
        Dictionary<string, string> QueryStrings = new Dictionary<string,string>();
        string _temp;
        QueryStrings["total participants"] = string.Format("SELECT COUNT(*) FROM participant where right(id, 1) ='{0}' and password <> 'test'", _school);

        _temp = "SELECT COUNT(DISTINCT id) FROM activity_log WHERE id not in ('no_session', 'counselor') and activity_code = '{1}' and right(id, 1) ='{0}'";
        QueryStrings["survey started"] = string.Format(_temp, new string[2] {_school, "SURVEY STARTED"});
        QueryStrings["survey completed"] = string.Format(_temp, new string[2] {_school, "SURVEY COMPLETED"});

        QueryStrings["visited dialog"] = string.Format(_temp, new string[2] {_school, "STUDENT: MSG THREAD READ"});

        QueryStrings["followup started"] = string.Format(_temp, new string[2] { _school, "FOLLOWUP SURVEY VISITED" });
        QueryStrings["followup completed"] = string.Format(_temp, new string[2] { _school, "FOLLOWUP SURVEY COMPLETED" });

        _temp = "SELECT COUNT(DISTINCT id) FROM status WHERE status_code = '{1}' AND status_value = '{2}' and right(id, 1)='{0}'";
        QueryStrings["full survey eligible"] = string.Format(_temp, new string[3] { _school, "FULL SURVEY ELIGIBILITY", "Y"});
        QueryStrings["intervention eligible"] = string.Format(_temp, new string[3] {_school, "INTERVENTION ELIGIBILITY", "Y"});
        QueryStrings["intervention group"] = string.Format(_temp, new string[3] {_school, "INTERVENTION", "1"});
        QueryStrings["control group"] = string.Format(_temp, new string[3] { _school, "INTERVENTION", "0" });

        _temp = "SELECT COUNT( {3}) FROM message WHERE {1} IS NOT NULL and {1} {2} and right({1}, 1) ='{0}' and {1} != 'aaaaaaaaam'";
        QueryStrings["students posted"] = string.Format(_temp, new string[4] { _school, "from_id", "not like 'C[0-9][0-9][0-9]'", "DISTINCT FROM_ID" });
        QueryStrings["student messages"] = string.Format(_temp, new string[4] {_school, "from_id", "not like 'C[0-9][0-9][0-9]'", "*" });
        QueryStrings["counselor messages"] = string.Format(_temp, new string[4] { _school, "to_id", "not like 'C[0-9][0-9][0-9]'", "*" });

        return QueryStrings;

    }

    // not needed, can't pass counselor id values anyways - just use the session variable
    public static string CounselorLogout(string CounselorId, string _school)
    {
        string Logout= "<A href='action.aspx?p=counselor_logout&c={0}&s={1}' style='color:blue'>Log Out</A>";
        return string.Format(Logout, new string[2] {CounselorId, _school});
    }

    public static string SanitizeHtml(string html)
    {
        string acceptable = "script|link|title";
        string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
        return Regex.Replace(html, stringPattern, "");
    }
}