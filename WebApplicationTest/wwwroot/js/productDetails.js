document.addEventListener("DOMContentLoaded", function () {
    let showCommentsButton = document.getElementById("showCommentsButton");
    let commentContainers = document.querySelectorAll(".comment-container");
    let areCommentsVisible = false;
    if (showCommentsButton) {

        showCommentsButton.addEventListener("click", function () {
            areCommentsVisible = !areCommentsVisible;

            commentContainers.forEach(function (container) {
                container.style.display = areCommentsVisible ? "block" : "none";
            });

            let commentCount = parseInt(showCommentsButton.getAttribute("data-comment-count"));

            showCommentsButton.textContent = areCommentsVisible ? "Hide Comments (" + commentCount + ")" : "Show Comments (" + commentCount + ")";
        });
    }
});



$(document).ready(function () {
    // Handle like button click using class selector
    $(document).on('click', '.likeButton', function () {
        let reviewId = $(this).data('review-id'); // Get review ID from data attribute
        let formData = $(this).closest('form').serialize(); // Serialize the form data

        $.ajax({
            url: $(this).closest('form').attr('action'), // Get form's action attribute
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response && typeof response.likeCount !== 'undefined' && typeof response.dislikeCount !== 'undefined') {
                    updateRatingCounters(reviewId, response);
                    // Disable both buttons after a successful like
                    disableButtons(reviewId);
                } else {
                    console.error('Invalid response format:', response);
                    alert('Unexpected response format. Please try again.');
                }
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                alert('An error occurred. Please try again.');
            }
        });
    });

    // Handle dislike button click using class selector
    $(document).on('click', '.dislikeButton', function () {
        let reviewId = $(this).data('review-id'); // Get review ID from data attribute
        let formData = $(this).closest('form').serialize(); // Serialize the form data

        $.ajax({
            url: $(this).closest('form').attr('action'), // Get form's action attribute
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response && typeof response.likeCount !== 'undefined' && typeof response.dislikeCount !== 'undefined') {
                    updateRatingCounters(reviewId, response);
                    // Disable both buttons after a successful dislike
                    disableButtons(reviewId);
                } else {
                    console.error('Invalid response format:', response);
                    alert('Unexpected response format. Please try again.');
                }
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                alert('An error occurred. Please try again.');
            }
        });
    });
});

// Function to update the like and dislike counters
function updateRatingCounters(reviewId, response) {
    $('.likeButton[data-review-id="' + reviewId + '"]').html('<i class="fa-solid fa-thumbs-up fa-sm"></i> ' + response.likeCount);
    $('.dislikeButton[data-review-id="' + reviewId + '"]').html('<i class="fa-solid fa-thumbs-down fa-sm"></i> ' + response.dislikeCount);
}

// Function to disable both buttons for a specific review
function disableButtons(reviewId) {
    $('.likeButton[data-review-id="' + reviewId + '"]').prop('disabled', true);
    $('.dislikeButton[data-review-id="' + reviewId + '"]').prop('disabled', true);
}