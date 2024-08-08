document.addEventListener('DOMContentLoaded', function () {
    const slider = document.querySelector('.slider'); // Select the slider container
    const slides = document.querySelectorAll('.slider> div'); // Select all slides (images)
    const slideCount = slides.length; // Number of slides
    const interval = 3000; // Time for each slide transition in milliseconds (3 seconds)
    
    let currentIndex = 0; // Index of the current slide
    let startX = 0; // Starting x coordinate for touch
    let endX = 0; // Ending x coordinate for touch

    function goToSlide(index) {
        const offset = slides[0].clientWidth * index; // Calculate the offset based on slide width
        slider.style.transform = `translateX(-${offset}px)`; // Apply the translation to show the correct slide
        currentIndex = index; // Update the current slide index
    }

    function nextSlide() {
        currentIndex = (currentIndex + 1) % slideCount; // Move to the next slide, wrap around if needed
        goToSlide(currentIndex); // Show the next slide
    }

    function previousSlide() {
        currentIndex = (currentIndex - 1 + slideCount) % slideCount; // Move to the previous slide, wrap around if needed
        goToSlide(currentIndex); // Show the previous slide
    }

    // Automatically transition to the next slide every few seconds
    setInterval(nextSlide, interval);

    // Optional: Add click event listeners for navigation dots
    const navLinks = document.querySelectorAll('.slider-nav a'); // Select all navigation dots
    navLinks.forEach((link, index) => {
        link.addEventListener('click', (event) => {
            event.preventDefault(); // Prevent the default anchor behavior
            goToSlide(index); // Show the slide corresponding to the clicked dot
        });
    });

    // Swipe handling
    slider.addEventListener('touchstart', function(event) {
        startX = event.touches[0].clientX; // Store the starting x coordinate
    });

    slider.addEventListener('touchend', function(event) {
        endX = event.changedTouches[0].clientX; // Store the ending x coordinate
        const diffX = endX - startX; // Calculate the difference
        if (Math.abs(diffX) > 50) { // If the swipe is significant
            if (diffX > 0) {
                previousSlide(); // Swipe right, go to the previous slide
            } else {
                nextSlide(); // Swipe left, go to the next slide
            }
        }
    });
});

    $(function(){
  
        $('[data-toggle=tooltip]').tooltip();
  
        $('.grid-view').click(function(){
    
        });
  
    });