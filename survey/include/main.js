function __setQuestions() {
	questions = document.getElementsByClassName('question');
	number = question.previousSibling;
	for (i = 0; i < questions.length; i++) {
		options = questions[i];
		if (option.tagName == "input") {
			option.onclick()
		}
	}
}