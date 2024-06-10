const slider = document.getElementById('cards_space');
const prevButton = document.getElementById('arrow_prev');
const nextButton = document.getElementById('arrow_next');
const slideWidth = 250 + 10; // ширина слайда плюс отступ

prevButton.addEventListener('click', () => {
    let currentLeft = parseInt(window.getComputedStyle(slider).left, 10) || 0;
    slider.style.left = (currentLeft - 150) + 'px';
});

nextButton.addEventListener('click', () => {
    let currentLeft = parseInt(window.getComputedStyle(slider).left, 10) || 0;
    slider.style.left = (currentLeft + 150) + 'px';
});