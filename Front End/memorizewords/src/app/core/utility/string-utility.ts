import { POLISH_TO_LATIN_DICTIONARY } from "../constants/alphabet";

export class StringCompare {

    static compareWithPolishAlphabet(value: string, polishWord: string): boolean {

        const polishToLatinDictionary = POLISH_TO_LATIN_DICTIONARY;

        for (let i = 0; i < value.length; i++) {
            if (value[i] !== polishToLatinDictionary[polishWord[i]]) {
                return false;
            }
        }

        return true;
    }

}