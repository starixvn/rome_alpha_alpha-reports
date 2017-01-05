const coreTestsTemplate = {
    tableWithCombinedColumns: `<table>
                                    <tr>
                                        <td class="column-a-combined">
                                            <ul>
                                                <li>Row 1 - Col 1</li>
                                                <li>Row 1 - Col 2</li>
                                                <li>Row 1 - Col 3</li>
                                            </ul>
                                        </td>
                                        <td>
                                            <ul>
                                                <li>Row 1 - Col 4</li>
                                                <li>Row 1 - Col 5</li>
                                                <li>Row 1 - Col 6</li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ul>
                                                <li>Row 2 - Col 1</li>
                                                <li>Row 2 - Col 2</li>
                                                <li>Row 2 - Col 3</li>
                                            </ul>
                                        </td>
                                        <td class="column-a-combined">
                                            <ul>
                                                <li>Row 2 - Col 4</li>
                                                <li>Row 2 - Col 5</li>
                                                <li>Row 2 - Col 6</li>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>`,
    tableWithSplittedColumns: `<table>
                                    <tr>
                                        <td class="hidden-stack-mode ">Row 1 - Col 1</td>
                                        <td class="hidden-stack-mode ">Row 1 - Col 2</td>
                                        <td class="hidden-stack-mode bold">Row 1 - Col 3</td>
                                        <td class="column-a-combined">
                                            <ul>
                                                <li>Row 1 - Col 1</li>
                                                <li>Row 1 - Col 2</li>
                                                <li>Row 1 - Col 3</li>
                                            </ul>
                                        </td>
                                        <td>
                                            <ul>
                                                <li>Row 1 - Col 4</li>
                                                <li>Row 1 - Col 5</li>
                                                <li>Row 1 - Col 6</li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ul>
                                                <li>Row 2 - Col 1</li>
                                                <li>Row 2 - Col 2</li>
                                                <li>Row 2 - Col 3</li>
                                            </ul>
                                        </td>
                                        <td class="hidden-stack-mode ">Row 2 - Col 4</td>
                                        <td class="hidden-stack-mode ">Row 2 - Col 5</td>
                                        <td class="hidden-stack-mode bold">Row 2 - Col 6</td>
                                        <td class="column-a-combined">
                                            <ul>
                                                <li>Row 2 - Col 4</li>
                                                <li>Row 2 - Col 5</li>
                                                <li>Row 2 - Col 6</li>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>`,
    tableNeedCombinedColumns: `<table>
                                    <tr>
                                        <td class="hidden-stack-mode column-1">Row 1 - Col 1</td>
                                        <td class="hidden-stack-mode ">Row 1 - Col 2</td>
                                        <td class="hidden-stack-mode bold">Row 1 - Col 3</td>
                                        <td>
                                            <ul>
                                                <li>Row 1 - Col 4</li>
                                                <li>Row 1 - Col 5</li>
                                                <li>Row 1 - Col 6</li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ul>
                                                <li>Row 2 - Col 1</li>
                                                <li>Row 2 - Col 2</li>
                                                <li>Row 2 - Col 3</li>
                                            </ul>
                                        </td>
                                        <td class="hidden-stack-mode column-1">Row 2 - Col 4</td>
                                        <td class="hidden-stack-mode ">Row 2 - Col 5</td>
                                        <td class="hidden-stack-mode bold">Row 2 - Col 6</td>
                                    </tr>
                                </table>`,
    tableAfterCombinedColumns: `<table>
                                    <tr>
                                        <td class="hidden-stack-mode column-1">Row 1 - Col 1</td>
                                        <td class="hidden-stack-mode ">Row 1 - Col 2</td>
                                        <td class="hidden-stack-mode bold">Row 1 - Col 3</td>
                                        <td class="visible-stack-mode combinedcell" colspan="1">
                                            <ul>
                                                <li>Row 1 - Col 1</li>
                                                <li>Row 1 - Col 2</li>
                                                <li>Row 1 - Col 3</li>
                                            </ul>
                                        </td>
                                        <td>
                                            <ul>
                                                <li>Row 1 - Col 4</li>
                                                <li>Row 1 - Col 5</li>
                                                <li>Row 1 - Col 6</li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ul>
                                                <li>Row 2 - Col 1</li>
                                                <li>Row 2 - Col 2</li>
                                                <li>Row 2 - Col 3</li>
                                            </ul>
                                        </td>
                                        <td class="hidden-stack-mode column-1">Row 2 - Col 4</td>
                                        <td class="hidden-stack-mode ">Row 2 - Col 5</td>
                                        <td class="hidden-stack-mode bold">Row 2 - Col 6</td>
                                        <td class="visible-stack-mode combinedcell" colspan="1">
                                            <ul>
                                                <li>Row 2 - Col 4</li>
                                                <li>Row 2 - Col 5</li>
                                                <li>Row 2 - Col 6</li>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>`
};