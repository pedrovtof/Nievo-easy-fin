/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        "primary": "#382d48",
        "secondary": "#2D3748",
        "accent-green": "#48bb78",
        "accent-red": "#f56565",
        "accent-blue": "#4299e1",
        "accent-orange": "#ed8936",
        "background-light": "#f7f6f7",
        "background-dark": "#19171b",
        "card-light": "#EDF2F7",
      },
      fontFamily: {
        "display": ["Manrope", "sans-serif"]
      },
      borderRadius: {
        "DEFAULT": "0.375rem", 
        "lg": "0.5rem", 
        "xl": "0.75rem", 
        "full": "9999px"
      },
    },
  },
  plugins: [],
}
