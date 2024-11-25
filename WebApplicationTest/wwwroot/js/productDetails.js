document.addEventListener("DOMContentLoaded", function () {
    let showCommentsButton = document.getElementById("showCommentsButton");
    let commentContainers = document.querySelectorAll(".comment-container");
    let areCommentsVisible = false;

    showCommentsButton.addEventListener("click", function () {
        areCommentsVisible = !areCommentsVisible;

        commentContainers.forEach(function (container) {
            container.style.display = areCommentsVisible ? "block" : "none";
        });

        let commentCount = parseInt(showCommentsButton.getAttribute("data-comment-count"));

        showCommentsButton.textContent = areCommentsVisible ? "Hide Comments (" + commentCount + ")" : "Show Comments (" + commentCount + ")";
    });
});

