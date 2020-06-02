import React, { Fragment } from 'react';
import { Link } from 'react-router-dom';
import { Badge } from 'react-bootstrap';
import { ShowDictLink } from './CommTools';

const WordInfoDisplay = ({ word }) => {
    console.log('WordInfoDisplay:'+word);
    if (word == null || word == undefined) return '';

    return (
        <table className='table table-striped table-bordered'>
            <tbody>
            <tr>
                    <td>Name:</td>
                    <td>
                        <a > {word.name} </a>
          
                        <Badge>{word.frequency}</Badge>
                        {' '}
                        <Link to={{ pathname: '/wordedit/' + word.name, state: { word: word } }} >
                            <button type="button" > Edit </button>
                        </Link>

                    </td>
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
    //if (word.totalViewed > 0)
    {
        infoList.push({ title: 'Start Time', value: new Date(word.startTime).toLocaleString() });
        infoList.push({ title: 'View Time', value: new Date(word.viewTime).toLocaleString()  });
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

export default WordInfoDisplay;
