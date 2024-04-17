// Preloader
const loader = document.querySelector(".loader");
window.addEventListener("load", () => {
  setTimeout(() => {
    loader.style.display = "none";
  }, 2000);
});

// Navigation Toggle
const hamburger = document.querySelector(".hamburger");
const navList = document.querySelector(".nav-list");

hamburger.addEventListener("click", () => {
  navList.classList.toggle("open");
});

// Fix Nav
const navigation = document.querySelector(".navigation");
const header = document.querySelector(".header");

window.addEventListener("scroll", () => {
  const scrollHeight = window.pageYOffset;
  if (scrollHeight > 200) {
    navigation.classList.add("fix");
    header.style.zIndex = "1000";
  } else {
    navigation.classList.remove("fix");
  }
});

// Swiper 1
const swiper1 = new Swiper(".slider-1", {
  autoplay: {
    delay: 3500,
  },
  loop: true,
  navigation: {
    nextEl: ".swiper-next",
    prevEl: ".swiper-prev",
  },
});


// Swiper 2
const swiper2 = new Swiper(".slider-2", {
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
});

// Swiper 3
const swiper3 = new Swiper(".slider-3", {
  effect: "coverflow",
  grabCursor: true,
  loop: true,
  centeredSlides: true,
  slidesPerView: "auto",
  navigation: {
    nextEl: ".custom-next",
    prevEl: ".custom-prev",
  },
});

// Scroll Reveal
const scroll = ScrollReveal({
  distance: "100px",
  duration: 2500,
  reset: true,
});

scroll.reveal(`.content h1, .content .btn`, {
  origin: "top",
});

scroll.reveal(`.about .col h1, .about .col p, .about .col .btn`, {
  origin: "left",
});

scroll.reveal(`.about .col:last-child,.contact .location,.more .col:last-child,.newsletter .form`, {
    origin: "right",
});

scroll.reveal(`.service img,.contact .title`, {
  origin: "top",
});

scroll.reveal(`.service .col,.trip .row`, {
  origin: "bottom",
});

scroll.reveal(`.trip .title,.more .col:first-child,.newsletter .col`, {
  origin: "left",
});

scroll.reveal(`.gallery h1, .gallery .container`, {
  origin: "top",
});
