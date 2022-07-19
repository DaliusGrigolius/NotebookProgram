export const endpoints = {
	Login: "https://localhost:44329/authorization/login",
	Register: "https://localhost:44329/authorization/register",
	RefreshTokens: "https://localhost:44329/authorization/refresh-tokens",
	AddCategory: "https://localhost:44329/Category/Add-new-category",
	EditCategory: "https://localhost:44329/Category/Edit-category-name",
	RemoveCategory: "https://localhost:44329/Category/Remove-category",
	GetAllCategories: "https://localhost:44329/Category/Get-all-categories",
	AddNote: "https://localhost:44329/Note/Create-a-note",
	ShowAllNotes: "https://localhost:44329/Note/Show-all-notes",
	FindNotesByTitle: "https://localhost:44329/Note/Find-notes-by-title",
	FindNotesByCategory:
		"https://localhost:44329/Note/Find-notes-by-category-name",
	EditNote: "https://localhost:44329/Note/Edit-the-note",
	AssignCategoryToNote:
		"https://localhost:44329/Note/Assign-category-to-the-note",
	AddImageToNote: "https://localhost:44329/Note/Add-image-to-the-note",
	RemoveNote: "https://localhost:44329/Note/Remove-the-note",
};
