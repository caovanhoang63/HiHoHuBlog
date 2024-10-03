/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{razor,html,cshtml}"],
    theme: {
        extend: {},
    },
    daisyui : {
        themes: ["light"],
    },
    plugins: [
        require('daisyui'),
    ],
}
