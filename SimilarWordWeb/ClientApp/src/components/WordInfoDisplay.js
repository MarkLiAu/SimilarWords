import React, { Fragment } from 'react';
import { Link } from 'react-router-dom';
import { Badge } from 'react-bootstrap';

const WordInfoDisplay = ({ word }) => {
    console.log(word);
    if (word == null || word == undefined) return '';
    return (
        <table className='table table-striped table-bordered'>
            <tbody>
            <tr>
                    <td>Name:</td><td> <ShowWordName name={word.name}></ShowWordName>    <Badge>{word.frequency}</Badge> </td>
            </tr>
            <tr>
                <td>pronounciation:</td><td>{word.pronounciation}</td>
            </tr>
                <tr>
                    <td>Similar words:</td><td>{word.similarWords}</td>
                </tr>
            <tr>
                <td>meaningShort:</td><td>{word.meaningShort}</td>
            </tr>
                <WordReviewInfo word={word}></WordReviewInfo>
                <tr>
                    <td>Onine dictionary</td><td><ShowDictLink name={word.name}></ShowDictLink></td>
                </tr>

                

            </tbody>
        </table>
    );
}

const WordReviewInfo = ({ word }) => {
    let infoList = [];
    infoList.push({ title: 'Total viewed', value: word.totalViewed + (word.totalViewed<=0 && word.viewInterval>=-1? ' (in memory list)':'') });
    if (word.totalViewed > 0) {
        infoList.push({ title: 'Start Time', value:word.startTime });
        infoList.push({ title: 'View Time', value:word.viewTime });
        infoList.push({ title: 'Interval', value:word.viewInterval });
        infoList.push({ title: 'Last Easiness', value:word.easiness });
    }
    return (
        infoList.map((x,idx) =>
            <tr key={idx}>
                <td>{x.title + ':'}</td><td>{x.value}</td>
            </tr>
        )
    
    );
}

const ShowWordName = ({ name }) => {
    //if (name == this.state.word2search) {
    //    return <b className='text-primary'>{name}</b>
    //}
    //else {
    return <Link to={'/WordSearch/' + name} >{name} </Link>
    //}
}



const ShowDictLink = ({ name } ) => {
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

export default WordInfoDisplay;
