import { endpoints } from "./endpoints.js";

const username = document.getElementById("username");
const password = document.getElementById("password");

let loginBtn;
let registerBtn;
let password2;
let validInputsCounter = 0;
loginBtnExists();
registerBtnExists();
password2Exists();

if (loginBtnExists()) {
	loginBtn.addEventListener("click", (e) => {
		e.preventDefault();
		login();
		validInputsCounter = 0;
	});
}

if (registerBtnExists()) {
	registerBtn.addEventListener("click", (e) => {
		e.preventDefault();
		register();
		validInputsCounter = 0;
	});
}

function login() {
	const loginErrors = document.getElementById("login-errors");
	loginErrors.innerText = "";
	checkInputs();
	if (validInputsCounter === 2) {
		axios
			.post(endpoints.Login, {
				Username: username.value,
				Password: password.value,
			})
			.then((response) => {
				setTokens(
					response.data.refreshToken.token,
					response.data.refreshToken.expires,
					response.data.token
				);
				redirectToIndexPage();
			})
			.catch((error) => {
				console.log(error);
				loginErrors.innerText = "Error: invalid username or/and password";
			});
	}
}

function register() {
	const registerErrors = document.getElementById("register-errors");
	registerErrors.innerText = "";
	checkInputs();
	if (validInputsCounter === 3) {
		axios
			.post(endpoints.Register, {
				Username: username.value,
				Password: password.value,
				ConfirmPassword: password2.value,
			})
			.then((response) => {
				console.log(response);
				redirectToLoginPage();
			})
			.catch((error) => {
				console.log("eroriukas: " + error);
				registerErrors.innerText = "Error: invalid username or/and password";
			});
	}
}

function checkInputs() {
	const usernameValue = username.value.trim();
	const passwordValue = password.value.trim();
	let password2Value;
	if (!!document.getElementById("password2")) {
		password2Value = password2.value.trim();
	}

	if (usernameValue === "") {
		setErrorFor(username, "Username cannot be blank");
	} else {
		setSuccessFor(username);
		validInputsCounter++;
	}

	if (passwordValue === "") {
		setErrorFor(password, "Password cannot be blank");
	} else if (passwordValue.length < 6) {
		setErrorFor(password, "Password cannot be shorter than 6 chars");
	} else {
		setSuccessFor(password);
		validInputsCounter++;
	}

	if (password2Exists()) {
		if (password2Value === "") {
			setErrorFor(password2, "Password2 cannot be blank");
		} else if (passwordValue !== password2Value) {
			setErrorFor(password2, "Passwords does not match");
		} else {
			setSuccessFor(password2);
			validInputsCounter++;
		}
	}
}

function password2Exists() {
	if (!!document.getElementById("password2")) {
		password2 = document.getElementById("password2");
		return true;
	} else {
		return false;
	}
}

function loginBtnExists() {
	if (!!document.getElementById("login-btn")) {
		loginBtn = document.getElementById("login-btn");
		return true;
	} else {
		return false;
	}
}

function registerBtnExists() {
	if (!!document.getElementById("register-btn")) {
		registerBtn = document.getElementById("register-btn");
		return true;
	} else {
		return false;
	}
}

function setErrorFor(input, message) {
	const formControl = input.parentElement;
	const small = formControl.querySelector("small");
	formControl.className = "form-control error";
	small.innerText = message;
}

function setSuccessFor(input) {
	const formControl = input.parentElement;
	formControl.className = "form-control success";
}

function setTokens(rToken, rtExpires, aToken) {
	document.cookie = `refreshToken=${rToken}; expires=${new Date(
		rtExpires
	)}; path=/;`;
	sessionStorage.setItem("accessToken", `${aToken}`);
}

function redirectToIndexPage() {
	window.location.assign("../index.html");
}

function redirectToLoginPage() {
	window.location.assign("./login.html");
}
