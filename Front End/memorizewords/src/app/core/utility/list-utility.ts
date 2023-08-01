export class ListUtility {

    public static findItemsFromPropertyValues<T>(array: T[], values: any[], property: string): T[] {

        if(!values){
            return array;
        }

        const foundItems: T[] = [];

        for (const value of values) {
            const foundItem = array.find((x) => (x as any)[property] === value);
            if (foundItem) {
                foundItems.push(foundItem);
            }
        }

        return foundItems;
    }

    public static removeItemsByPropertyValues<T>(array: T[], values: number[], property: string): T[] {
        if(!values){
            return array;
        }
        
        return array.filter((x) => !values.includes((x as any)[property]));
    }

}