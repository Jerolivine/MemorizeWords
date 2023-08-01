export class ListUtility {

    public static findItemsFromIndices<T>(array: T[], values: number[], property: string): T[] {
        const foundItems: T[] = [];

        for (const value of values) {
            const foundItem = array.find((x) => (x as any)[property] === value);
            if (foundItem) {
                foundItems.push(foundItem);
            }
        }

        return foundItems;
    }

    public static removeItemsByIndices<T>(array: T[], indicesToRemove: number[]): T[] {
        return array.filter((_, index) => !indicesToRemove.includes(index));
    }
}