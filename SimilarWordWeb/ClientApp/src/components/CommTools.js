import React, { Fragment } from 'react';

export const ShowDictLink = ({ name }) => {
    return (
        <Fragment>
            <a title="Collins" href={'https://www.collinsdictionary.com/dictionary/english/' + name} target="_blank">Coll</a>
            <span>|</span>
            <a title='Longman' href={'https://www.ldoceonline.com/dictionary/' + name} target="_blank">Long</a>
            <span>|</span>
            <a title='Merriam Webster' href={'https://www.merriam-webster.com/dictionary/' + name} target="_blank">Merr</a>
            <span>|</span>
            <a title='Oxford Learners' href={'https://www.oxfordlearnersdictionaries.com/definition/english/' + name + '_1'} target="_blank">Oxford</a>
            <span>|</span>
            <a title='Cambridge' href={'https://dictionary.cambridge.org/dictionary/english/' + name} target="_blank">Camb</a>
            <span>|</span>
            <a title='Macmilland' href={'https://www.macmillandictionary.com/dictionary/british/' + name + '_1'} target="_blank">Macm</a>
            <span>|</span>
            <a title='Lexico' href={'https://www.lexico.com/definition/' + name} target="_blank">Lexi</a>
            <span>|</span>
            <a title='Iciba' href={'http://www.iciba.com/word?w=' + name} target="_blank">Iciba</a>
        </Fragment>
    )
}

