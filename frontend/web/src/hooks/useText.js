import texts from '../locales/texts.json';

export const useText = () => {
  const t = (key) => {
    const keys = key.split('.');
    let result = texts;
    for (const k of keys) {
      if (result && result[k]) {
        result = result[k];
      } else {
        console.warn(`Text key not found: ${key}`);
        return key;
      }
    }
    return result;
  };
  return { t };
};
