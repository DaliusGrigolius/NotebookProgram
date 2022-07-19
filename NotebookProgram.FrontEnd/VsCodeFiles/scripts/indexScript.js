import { endpoints } from "./endpoints.js";

const outputDiv = document.getElementById("container");
document.addEventListener("DOMContentLoaded", start);

function start() {
	const refreshTokenValue = getCookieValueByName("refreshToken");
	if (refreshTokenValue === null) {
		redirectToLoginPage();
	}
	refreshTokens(refreshTokenValue);
	const accessTokenValueRefreshed = window.sessionStorage.getItem("accessToken");
	getData(accessTokenValueRefreshed);
}

function getData(accessTokenValue) {
	const options = {
		headers: {
			Authorization: `Bearer ${accessTokenValue}`,
			"content-type": "application/json",
		},
	};
	axios
		.get(endpoints.ShowAllNotes, options)
		.then((response) => {
			renderCards(response.data);
			createMenu();
		})
		.catch((error) => {
			console.log(error);
		});
}

function createMenu() {
	const headerDiv = document.querySelector("header");
	const menuDiv = document.createElement("div");

	const showAllNotesButton = document.createElement("button");
	const newNoteButton = document.createElement("button");
	const newCategoryButton = document.createElement("button");
	const filterByCategoryButton = document.createElement("button");
	const filterByTitleButton = document.createElement("button");
	const editCategoryButton = document.createElement("button");
	const removeCategoryButton = document.createElement("button");
	const logoutButton = document.createElement("button");

	showAllNotesButton.innerText = "Show all notes";
	newNoteButton.innerText = "Create a note";
	newCategoryButton.innerText = "Create new category";
	filterByCategoryButton.innerText = "Filter by category";
	filterByTitleButton.innerText = "Filter by title";
	editCategoryButton.innerText = "Edit category name";
	removeCategoryButton.innerText = "Remove category";
	logoutButton.innerText = "Log out";

	showAllNotesButton.setAttribute("id", "logout");
	showAllNotesButton.setAttribute("class", "showAllNotes");
	menuDiv.setAttribute("class", "menuDiv");
	newNoteButton.setAttribute("class", "addNote");
	newNoteButton.setAttribute("id", "logout");
	newCategoryButton.setAttribute("class", "addCategory");
	newCategoryButton.setAttribute("id", "logout");
	filterByCategoryButton.setAttribute("class", "filterByCategory");
	filterByCategoryButton.setAttribute("id", "logout");
	filterByTitleButton.setAttribute("class", "filterByTitle");
	filterByTitleButton.setAttribute("id", "logout");
	editCategoryButton.setAttribute("class", "editCategory");
	editCategoryButton.setAttribute("id", "logout");
	removeCategoryButton.setAttribute("class", "removeCategory");
	removeCategoryButton.setAttribute("id", "logout");
	logoutButton.setAttribute("id", "logout");
	logoutButton.setAttribute("class", "logout1");

	menuDiv.append(
		showAllNotesButton,
		newNoteButton,
		newCategoryButton,
		editCategoryButton,
		removeCategoryButton,
		filterByCategoryButton,
		filterByTitleButton
	);
	headerDiv.append(logoutButton, menuDiv);

	logoutButton.addEventListener("click", logout);
	newNoteButton.addEventListener("click", handleAddNewNote);
	newCategoryButton.addEventListener("click", handleAddNewCategory);
	editCategoryButton.addEventListener("click", getAllCategories);
	removeCategoryButton.addEventListener("click", getAllCategories1);
	filterByCategoryButton.addEventListener("click", getAllCategories2);
	filterByTitleButton.addEventListener("click", handleGetNotesByTitle);
	showAllNotesButton.addEventListener("click", refreshPage);
}

function refreshPage() {
	setTimeout(function () {
		window.location.reload();
	});
}

function handleGetNotesByTitle() {
	const accessTokenValue = window.sessionStorage.getItem("accessToken");
	const options = {
		headers: {
			Authorization: `Bearer ${accessTokenValue}`,
			"content-type": "application/json",
		},
	};
	axios
		.get(endpoints.ShowAllNotes, options)
		.then((res) => {
			createModalForHandlingFilterByTitle(res.data);
		})
		.catch((error) => {
			console.log(error);
		});
}

function createModalForHandlingFilterByTitle(notes) {
	const container = document.querySelector("main");

	const dialogNode = document.createElement("dialog");
	const closeDialogButton = document.createElement("button");

	closeDialogButton.innerText = "Close";

	dialogNode.setAttribute("class", "modal8");
	dialogNode.setAttribute("id", "modal8");
	closeDialogButton.setAttribute("class", "closeDialogButton");

	Array.from(notes).forEach((note) => {
		const noteButton = document.createElement("button");
		noteButton.innerText = note.title;
		noteButton.setAttribute("class", "noteTitleButton");
		noteButton.setAttribute("id", `${note.id}`);

		dialogNode.append(noteButton);

		noteButton.addEventListener("click", () => {
			const accessTokenValue = window.sessionStorage.getItem("accessToken");
			const options = {
				headers: {
					Authorization: `Bearer ${accessTokenValue}`,
					"content-type": "application/json",
				},
				params: {
					noteTitle: note.title,
				},
			};
			axios
				.get(endpoints.FindNotesByTitle, options)
				.then((res) => {
					outputDiv.innerHTML = "";
					renderCards(res.data);
				})
				.catch((error) => {
					if (error.response.status == 404) {
						showModalMessage(error.response.data);
						dialogNode.close();
						dialogNode.remove();
					}
					console.log(error);
				});
			dialogNode.close();
			dialogNode.remove();
		});
	});

	dialogNode.append(closeDialogButton);
	container.append(dialogNode);
	dialogNode.showModal();

	closeDialogButton.addEventListener("click", () => {
		dialogNode.close();
		dialogNode.remove();
	});
}

function getAllCategories2() {
	const accessTokenValue = window.sessionStorage.getItem("accessToken");
	const options = {
		headers: {
			Authorization: `Bearer ${accessTokenValue}`,
			"content-type": "application/json",
		},
	};
	axios
		.get(endpoints.GetAllCategories, options)
		.then((res) => {
			createModalForHandlingFilter(res.data);
		})
		.catch((error) => {
			console.log(error);
		});
}

function createModalForHandlingFilter(allCategories) {
	const container = document.querySelector("main");

	const dialogNode = document.createElement("dialog");
	const closeDialogButton = document.createElement("button");

	closeDialogButton.innerText = "Close";

	dialogNode.setAttribute("class", "modal6");
	dialogNode.setAttribute("id", "modal6");
	closeDialogButton.setAttribute("class", "closeDialogButton");

	Array.from(allCategories).forEach((cat) => {
		const categoryButtonNode = document.createElement("button");
		categoryButtonNode.innerText = cat.name;
		categoryButtonNode.setAttribute("class", "catNameButton");
		categoryButtonNode.setAttribute("id", `${cat.id}`);

		dialogNode.append(categoryButtonNode);

		categoryButtonNode.addEventListener("click", () => {
			const accessTokenValue = window.sessionStorage.getItem("accessToken");
			const options = {
				headers: {
					Authorization: `Bearer ${accessTokenValue}`,
					"content-type": "application/json",
				},
				params: {
					categoryName: cat.name,
				},
			};
			axios
				.get(endpoints.FindNotesByCategory, options)
				.then((res) => {
					outputDiv.innerHTML = "";
					renderCards(res.data);
				})
				.catch((error) => {
					if (error.response.status == 404) {
						showModalMessage(error.response.data);
						dialogNode.close();
						dialogNode.remove();
					}
					console.log(error);
				});
			dialogNode.close();
			dialogNode.remove();
		});
	});

	dialogNode.append(closeDialogButton);
	container.append(dialogNode);
	dialogNode.showModal();

	closeDialogButton.addEventListener("click", () => {
		dialogNode.close();
		dialogNode.remove();
	});
}

function showModalMessage(data) {
	const container = document.querySelector("main");

	const dialogNode = document.createElement("dialog");
	const closeDialogButton = document.createElement("button");
	const mssgDiv = document.createElement("div");

	closeDialogButton.innerText = "Ok";
	mssgDiv.innerText = `${data}`;

	dialogNode.setAttribute("class", "modal7");
	dialogNode.setAttribute("id", "modal7");
	closeDialogButton.setAttribute("class", "closeDialogButton");

	dialogNode.append(mssgDiv, closeDialogButton);
	container.append(dialogNode);

	dialogNode.showModal();
	closeDialogButton.addEventListener("click", () => {
		dialogNode.close();
		dialogNode.remove();
	});
}

function getAllCategories1() {
	const accessTokenValue = window.sessionStorage.getItem("accessToken");
	const options = {
		headers: {
			Authorization: `Bearer ${accessTokenValue}`,
			"content-type": "application/json",
		},
	};
	axios
		.get(endpoints.GetAllCategories, options)
		.then((res) => {
			handleRemoveCategoryName(res.data);
		})
		.catch((error) => {
			console.log(error);
		});
}

function handleRemoveCategoryName(data) {
	const container = document.querySelector("main");

	const dialogNode = document.createElement("dialog");
	const closeDialogNodeButton = document.createElement("button");

	closeDialogNodeButton.innerText = "Close";

	dialogNode.setAttribute("class", "modal4");
	dialogNode.setAttribute("id", "modal4");
	closeDialogNodeButton.setAttribute("class", "closeDialogButton");

	data.forEach((cat) => {
		const categoryButtonNode = document.createElement("button");
		categoryButtonNode.innerText = cat.name;
		categoryButtonNode.setAttribute("class", "catNameButton");
		categoryButtonNode.setAttribute("id", `${cat.id}`);

		dialogNode.append(categoryButtonNode, closeDialogNodeButton);

		categoryButtonNode.addEventListener("click", () => {
			const accessTokenValue = window.sessionStorage.getItem("accessToken");
			const options = {
				headers: {
					Authorization: `Bearer ${accessTokenValue}`,
					"content-type": "application/json",
				},
				params: {
					id: cat.id,
				},
			};
			axios
				.delete(endpoints.RemoveCategory, options)
				.then((res) => {
					console.log(res);
				})
				.catch((error) => {
					console.log(error);
				});

			categoryButtonNode.remove();
		});
	});
	container.append(dialogNode);

	dialogNode.showModal();
	closeDialogNodeButton.addEventListener("click", () => {
		dialogNode.close();
		dialogNode.remove();
		setTimeout(function () {
			window.location.reload();
		}, 1000);
	});
}

function getAllCategories() {
	const accessTokenValue = window.sessionStorage.getItem("accessToken");
	const options = {
		headers: {
			Authorization: `Bearer ${accessTokenValue}`,
			"content-type": "application/json",
		},
	};
	axios
		.get(endpoints.GetAllCategories, options)
		.then((res) => {
			console.log(res);
			handleEditCategoryName(res.data);
		})
		.catch((error) => {
			console.log(error);
		});
}

function handleEditCategoryName(data) {
	const container = document.querySelector("main");

	const dialogNode = document.createElement("dialog");
	const closeDialogNodeButton = document.createElement("button");

	closeDialogNodeButton.innerText = "Close";

	dialogNode.setAttribute("class", "modal3");
	dialogNode.setAttribute("id", "modal3");
	closeDialogNodeButton.setAttribute("class", "closeDialogButton");

	data.forEach((cat) => {
		const categoryDivNode = document.createElement("div");
		categoryDivNode.innerText = cat.name;
		categoryDivNode.setAttribute("class", "catNameDiv");
		categoryDivNode.setAttribute("id", `${cat.id}`);
		categoryDivNode.setAttribute("contentEditable", "true");

		dialogNode.append(categoryDivNode);

		categoryDivNode.addEventListener("blur", () => {
			if (categoryDivNode.innerText == "") {
				categoryDivNode.innerText = cat.name;
				return;
			} else if (categoryDivNode.innerText == cat.name) {
				return;
			}

			const accessTokenValue = window.sessionStorage.getItem("accessToken");
			const options = {
				headers: {
					Authorization: `Bearer ${accessTokenValue}`,
					"content-type": "application/json",
				},
				params: {
					id: cat.id,
					newCategoryName: categoryDivNode.innerText,
				},
			};
			axios
				.put(endpoints.EditCategory, {}, options)
				.then((res) => {
					console.log(res);
				})
				.catch((error) => {
					console.log(error);
				});
		});
	});

	dialogNode.append(closeDialogNodeButton);
	container.append(dialogNode);

	dialogNode.showModal();
	closeDialogNodeButton.addEventListener("click", () => {
		dialogNode.close();
		dialogNode.remove();
		setTimeout(function () {
			window.location.reload();
		}, 1000);
	});
}

function handleAddNewCategory() {
	const container = document.querySelector("main");

	const dialogNode = document.createElement("dialog");
	const catName = document.createElement("div");
	const createCategory = document.createElement("button");

	createCategory.innerText = "Create!";
	catName.innerText = "Category name goes here..";

	createCategory.setAttribute("class", "createCategory");
	dialogNode.setAttribute("class", "modal2");
	dialogNode.setAttribute("id", "modal2");
	catName.setAttribute("id", "newCatName");
	catName.setAttribute("contentEditable", "true");

	dialogNode.append(catName, createCategory);
	container.append(dialogNode);

	dialogNode.showModal();

	catName.addEventListener(
		"click",
		() => {
			catName.innerText = "";
		},
		{ once: true }
	);

	createCategory.addEventListener("click", () => {
		const catNameValue = catName.innerText;
		if (catNameValue == "") {
			return;
		}

		const accessTokenValue = window.sessionStorage.getItem("accessToken");
		const options = {
			headers: {
				Authorization: `Bearer ${accessTokenValue}`,
				"content-type": "application/json",
			},
			params: {
				categoryName: catNameValue,
			},
		};
		axios
			.post(endpoints.AddCategory, {}, options)
			.then((res) => {
				console.log(res);
			})
			.catch((error) => {
				console.log(error);
			});

		dialogNode.close();
		dialogNode.remove();
		setTimeout(function () {
			window.location.reload();
		}, 1000);
	});
}

function handleAddNewNote() {
	const container = document.querySelector("main");

	const dialogNode = document.createElement("dialog");
	const title = document.createElement("div");
	const content = document.createElement("div");
	const createNote = document.createElement("button");

	createNote.innerText = "Create!";
	title.innerText = "Title goes here..";
	content.innerText = "Content goes here..";

	createNote.setAttribute("class", "createNote");
	dialogNode.setAttribute("class", "modal1");
	dialogNode.setAttribute("id", "modal1");
	title.setAttribute("id", "newNoteTitle");
	title.setAttribute("contentEditable", "true");
	content.setAttribute("id", "newNoteContent");
	content.setAttribute("contentEditable", "true");

	dialogNode.append(title, content, createNote);
	container.append(dialogNode);

	dialogNode.showModal();

	title.addEventListener(
		"click",
		() => {
			title.innerText = "";
		},
		{ once: true }
	);
	content.addEventListener(
		"click",
		() => {
			content.innerText = "";
		},
		{ once: true }
	);
	createNote.addEventListener("click", () => {
		const titleValue = title.innerText;
		const contentValue = content.innerText;
		if (titleValue == "" || contentValue == "") {
			return;
		}

		const accessTokenValue = window.sessionStorage.getItem("accessToken");
		const options = {
			headers: {
				Authorization: `Bearer ${accessTokenValue}`,
				"content-type": "application/json",
			},
			params: {
				title: titleValue,
				content: contentValue,
			},
		};
		axios
			.post(endpoints.AddNote, {}, options)
			.then((res) => {
				console.log(res);
			})
			.catch((error) => {
				console.log(error);
			});

		dialogNode.close();
		dialogNode.remove();
		setTimeout(function () {
			window.location.reload();
		}, 1000);
	});
}

function renderCards(data) {
	Array.from(data).forEach((card) => {
		const noteCard = createCard(card);
		outputDiv.append(noteCard);
	});
	addListenersForDelete();
	addListenersForTitleChange();
	addListenerForContentChange();
	addListenerForAddImageButton();
	addListenerForAssignCategory();
}

function addListenerForAssignCategory() {
	const assignCategoryButtons = document.querySelectorAll(
		".assignCategoryButton"
	);
	assignCategoryButtons.forEach((cat) => {
		cat.addEventListener("click", (e) => {
			const id = e.target.id;

			const accessTokenValue = window.sessionStorage.getItem("accessToken");
			const options = {
				headers: {
					Authorization: `Bearer ${accessTokenValue}`,
					"content-type": "application/json",
				},
			};
			axios
				.get(endpoints.GetAllCategories, options)
				.then((res) => {
					handleAssigning(res.data, id);
				})
				.catch((error) => {
					console.log(error);
				});
		});
	});
}

function handleAssigning(data, cardId) {
	const container = document.querySelector("#container");

	const dialogNode = document.createElement("dialog");
	const closeDialogNodeButton = document.createElement("button");

	closeDialogNodeButton.innerText = "Close";

	dialogNode.setAttribute("class", "modal5");
	dialogNode.setAttribute("id", "modal5");
	closeDialogNodeButton.setAttribute("class", "closeDialogButton");

	data.forEach((cat) => {
		const categoryButtonNode = document.createElement("button");
		categoryButtonNode.innerText = cat.name;
		categoryButtonNode.setAttribute("class", "catNameButton");
		categoryButtonNode.setAttribute("id", `${cat.id}`);

		dialogNode.append(categoryButtonNode);

		categoryButtonNode.addEventListener("click", () => {
			const accessTokenValue = window.sessionStorage.getItem("accessToken");
			const options = {
				headers: {
					Authorization: `Bearer ${accessTokenValue}`,
					"content-type": "application/json",
				},
				params: {
					noteId: cardId,
					categoryId: cat.id,
				},
			};
			axios
				.put(endpoints.AssignCategoryToNote, {}, options)
				.then((res) => {
					console.log(res);
				})
				.catch((error) => {
					console.log(error);
				});
			dialogNode.close();
			dialogNode.remove();
			setTimeout(function () {
				window.location.reload();
			}, 500);
		});
	});
	dialogNode.append(closeDialogNodeButton);
	container.append(dialogNode);

	dialogNode.showModal();
	closeDialogNodeButton.addEventListener("click", () => {
		dialogNode.close();
		dialogNode.remove();
		setTimeout(function () {
			window.location.reload();
		}, 500);
	});
}

function addListenerForAddImageButton() {
	const addImageButtons = document.querySelectorAll(".addImageButton");
	addImageButtons.forEach((btn) => {
		btn.addEventListener("click", (ev) => {
			ev.preventDefault();
			const container = document.querySelector(".noteCard");

			const dialogNode = document.createElement("dialog");
			dialogNode.setAttribute("class", "modal");
			dialogNode.setAttribute("id", "modal");

			const inputFileNode = document.createElement("input");
			inputFileNode.setAttribute("type", "file");
			inputFileNode.setAttribute("class", "file-upload");
			inputFileNode.setAttribute("accept", "image/*");

			const uploadButton = document.createElement("button");
			uploadButton.innerText = "Upload!";
			uploadButton.setAttribute("class", "uploadButton");

			const closeModalButton = document.createElement("button");
			closeModalButton.innerText = "Close";
			closeModalButton.setAttribute("class", "closeModal");

			dialogNode.append(inputFileNode, closeModalButton, uploadButton);
			container.append(dialogNode);

			dialogNode.showModal();

			closeModalButton.addEventListener("click", () => {
				dialogNode.close();
			});

			inputFileNode.addEventListener("change", (e) => {
				const uploadedImage = e.target.files[0];
				var reader = new FileReader();
				reader.readAsDataURL(uploadedImage);

				const accessTokenValue = window.sessionStorage.getItem("accessToken");
				const noteId = ev.target.id;
				console.log(noteId);

				const formData = new FormData();
				formData.append("file", uploadedImage);

				const options = {
					headers: {
						Authorization: `Bearer ${accessTokenValue}`,
						"content-type": "application/json",
					},
					params: {
						Id: noteId,
						file: uploadedImage,
					},
				};
				axios
					.post(endpoints.AddImageToNote, formData, options)
					.then((res) => {
						console.log(res.data);
						console.log(res.data.url);
					})
					.catch((error) => {
						console.log(error);
					});
				//--------------------------------
				const imgDiv = document.querySelector(".imagesDiv");
				const newImg = document.createElement("img");
				newImg.setAttribute("class", "addedImage");
				newImg.src = URL.createObjectURL(uploadedImage);
				imgDiv.onload = () => {
					URL.revokeObjectURL(imgDiv.src);
				};
				imgDiv.append(newImg);
				//-----------------------------------
				dialogNode.close();
				dialogNode.remove();
			});
		});
	});
}

function addListenerForContentChange() {
	const contentDivs = document.querySelectorAll(".content");
	const accessTokenValue = window.sessionStorage.getItem("accessToken");

	contentDivs.forEach((cont) => {
		const originalContent = cont.innerHTML;
		const originalTitle = cont.previousElementSibling.innerHTML;

		cont.addEventListener("blur", (ev) => {
			const newContent = ev.target.innerHTML;
			if (newContent == "") {
				cont.innerHTML == originalContent;
			} else {
				const options = {
					headers: {
						Authorization: `Bearer ${accessTokenValue}`,
						"content-type": "application/json",
					},
					params: {
						noteId: cont.id,
						newTitle: originalTitle,
						newContent: newContent,
					},
				};
				axios
					.put(endpoints.EditNote, {}, options)
					.then((response) => {
						console.log(response);
					})
					.catch((error) => {
						console.log(error);
					});
			}
		});
	});
}

function addListenersForTitleChange() {
	const titleDivs = document.querySelectorAll(".titleh3");
	const accessTokenValue = window.sessionStorage.getItem("accessToken");

	titleDivs.forEach((title) => {
		const originalTitle = title.innerHTML;
		const originalContent = title.nextSibling.innerHTML;

		title.addEventListener("blur", (ev) => {
			const newTitle = ev.target.innerHTML;
			if (newTitle == "") {
				title.innerHTML = originalTitle;
			} else {
				const options = {
					headers: {
						Authorization: `Bearer ${accessTokenValue}`,
						"content-type": "application/json",
					},
					params: {
						noteId: title.id,
						newTitle: newTitle,
						newContent: originalContent,
					},
				};
				axios
					.put(endpoints.EditNote, {}, options)
					.then((response) => {
						console.log(response);
					})
					.catch((error) => {
						console.log(error);
					});
			}
		});
	});
}

function addListenersForDelete() {
	const btns = document.querySelectorAll("a");
	btns.forEach((btn) => {
		btn.addEventListener("click", (event) => {
			const id = event.target.id;
			event.target.parentNode.parentNode.removeChild(event.target.parentNode);

			const accessTokenValue = window.sessionStorage.getItem("accessToken");
			const options = {
				headers: {
					Authorization: `Bearer ${accessTokenValue}`,
					"content-type": "application/json",
				},
				params: { noteId: `${id}` },
			};
			axios
				.delete(endpoints.RemoveNote, options)
				.then((response) => {
					console.log(response);
				})
				.catch((error) => {
					console.log(error);
				});
		});
	});
}

const createCard = (card) => {
	const noteCard = document.createElement("div");
	const deleteButton = document.createElement("a");
	const title = document.createElement("h3");
	const content = document.createElement("div");
	const imagesDiv = document.createElement("div");
	const categoriesDiv = document.createElement("div");

	title.innerText = card.title;
	content.innerText = card.content;

	noteCard.setAttribute("id", `${card.id}`);
	noteCard.setAttribute("class", "noteCard");
	deleteButton.setAttribute("id", `${card.id}`);
	title.setAttribute("id", `${card.id}`);
	title.setAttribute("class", "titleh3");
	title.setAttribute("contentEditable", "true");
	content.setAttribute("id", `${card.id}`);
	content.setAttribute("class", "content");
	content.setAttribute("contentEditable", "true");
	imagesDiv.setAttribute("id", `${card.id}`);
	imagesDiv.setAttribute("class", "imagesDiv");
	categoriesDiv.setAttribute("id", `${card.id}`);
	categoriesDiv.setAttribute("class", "categoriesDiv");

	card.images.forEach((image) => {
		const img = document.createElement("img");

		img.src = `data:image/png;base64, ${image.byte}`;
		img.setAttribute("class", "addedImage");

		imagesDiv.append(img);
	});

	card.categories.forEach((category) => {
		const categorySpan = document.createElement("span");
		categorySpan.setAttribute("class", "categorySpan");
		categorySpan.innerText = category.name;

		categoriesDiv.append(categorySpan);
	});

	const buttons = createButtons(card.id);
	noteCard.append(
		deleteButton,
		title,
		content,
		categoriesDiv,
		imagesDiv,
		buttons
	);

	return noteCard;
};

const createButtons = (cardId) => {
	const buttonsDiv = document.createElement("div");
	const addImageToCardButton = document.createElement("button");
	const assignCategoryToCardButton = document.createElement("button");

	addImageToCardButton.innerText = "Add image";
	assignCategoryToCardButton.innerText = "Assign category";

	addImageToCardButton.setAttribute("class", "addImageButton");
	addImageToCardButton.setAttribute("id", `${cardId}`);
	assignCategoryToCardButton.setAttribute("class", "assignCategoryButton");
	assignCategoryToCardButton.setAttribute("id", `${cardId}`);
	buttonsDiv.setAttribute("class", "buttonsDiv");

	buttonsDiv.append(addImageToCardButton, assignCategoryToCardButton);

	return buttonsDiv;
};

function getCookieValueByName(cname) {
	let name = cname + "=";
	let decodedCookie = decodeURIComponent(document.cookie);
	let ca = decodedCookie.split(";");
	for (let i = 0; i < ca.length; i++) {
		let c = ca[i];
		while (c.charAt(0) == " ") {
			c = c.substring(1);
		}
		if (c.indexOf(name) == 0) {
			return c.substring(name.length, c.length);
		}
	}
	return null;
}

function refreshTokens(refreshTokenValue) {
	const options = {
		headers: { "content-type": "application/json" },
		params: { refreshToken: `${refreshTokenValue}` },
	};
	axios
		.post(endpoints.RefreshTokens, {}, options)
		.then((response) => {
			setTokens(
				response.data.refreshToken.token,
				response.data.refreshToken.expires,
				response.data.token
			);
		})
		.catch((error) => {
			redirectToLoginPage();
			console.log(error);
		});
}

function setTokens(rToken, rtExpires, aToken) {
	document.cookie = `refreshToken=${rToken}; expires=${new Date(
		rtExpires
	)}; path=/`;
	sessionStorage.setItem("accessToken", `${aToken}`);
}

function deleteCookie(cname, cvalue, exdays) {
	const d = new Date();
	d.setTime(d.getTime() + exdays * 24 * 60 * 60 * 1000);
	let expires = "expires=" + d.toUTCString();
	document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function redirectToLoginPage() {
	window.location.assign("/pages/login.html");
}

function logout() {
	deleteCookie("refreshToken", "", -1);
	sessionStorage.clear();
	redirectToLoginPage();
}
