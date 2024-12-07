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
    $('#likeButton').click(function () {
        var formData = $('#likeForm').serialize(); // Serialize the form data
        var reviewId = $('#likeForm input[name="LikeForm.ReviewID"]').val(); // Get the review ID

        $.ajax({
            url: $('#likeForm').attr('action'), // Get the form's action attribute
            type: 'POST',
            data: formData,
            success: function (response) {
                // Call updateRatingCounters to refresh like/dislike counts
                updateRatingCounters(reviewId);
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                alert('An error occurred. Please try again.');
            }
        });
    });

    $('#dislikeButton').click(function () {
        var formData = $('#dislikeForm').serialize(); // Serialize the form data
        var reviewId = $('#dislikeForm input[name="DislikeForm.ReviewID"]').val(); // Get the review ID

        $.ajax({
            url: $('#dislikeForm').attr('action'), // Get the form's action attribute
            type: 'POST',
            data: formData,
            success: function (response) {
                // Call updateRatingCounters to refresh like/dislike counts
                updateRatingCounters(reviewId);
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                alert('An error occurred. Please try again.');
            }
        });
    });
});

function updateRatingCounters(reviewId) {
    $.ajax({
        url: '/Rating/GetReviewInformation',
        type: 'GET',
        data: { reviewID: reviewId },
        success: function (response) {
            if (response.success) {
                // Update the counters dynamically
                $('#likeButton').html(`<i class="fa-solid fa-thumbs-up fa-sm"></i> ${response.likeCount}`);
                $('#dislikeButton').html(`<i class="fa-solid fa-thumbs-down fa-sm"></i> ${response.dislikeCount}`);

                // Disable the buttons to prevent further clicks
                $('#likeButton').prop('disabled', true);
                $('#dislikeButton').prop('disabled', true);
            } else {
                alert('Failed to fetch ratings information.');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error fetching ratings info:', error);
            alert('Failed to update counters.');
        }
    });
}